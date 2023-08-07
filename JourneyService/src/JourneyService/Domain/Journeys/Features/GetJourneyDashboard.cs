
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Mappings;
using JourneyService.Domain.Journeys.Services;
using JourneyService.Infrastructure.Grpc;
using JourneyService.Infrastructure.Models;
using JourneyService.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JourneyService.Domain.Journeys.Features
{
    public class GetJourneyDashboard
    {
        public sealed class Query : IRequest<PagedList<JourneyDashboardDto>>
        {
            public readonly JourneyDashboardParametersDto QueryParameters;

            public Query(JourneyDashboardParametersDto queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }


        public sealed class Handler : IRequestHandler<Query, PagedList<JourneyDashboardDto>>
        {
            private readonly IJourneyRepository _journeyRepository;
            private readonly IGrpcUserClient _grpcUserClient;
            private readonly ILogger<Handler> _logger;
            private readonly IMemoryCache _cache;
            public Handler(IJourneyRepository journeyRepository, IGrpcUserClient grpcUserClient, ILogger<Handler> logger, IMemoryCache cache)
            {
                _journeyRepository = journeyRepository;
                _grpcUserClient = grpcUserClient;
                _logger = logger;
                _cache = cache;
            }

            /// <summary>
            /// Get the journey dashboard data, if the request demands user filtering, retrive user data first so that we can take advantage of the user pagination,
            /// otherwise filter by journeys first so we can take advantage of the journeys pagination
            /// </summary>
            public async Task<PagedList<JourneyDashboardDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                string cacheKey = request.QueryParameters.GenerateCacheKey();

                _logger.LogInformation("Retrieving journey dashboard with request{@request}", request);
                // Try to get the result from the cache
                if (_cache.TryGetValue(cacheKey, out PagedList<JourneyDashboardDto> cachedResult))
                {
                    return cachedResult;
                }

                IQueryable<Journey> collection = _journeyRepository.Query()
                    .Include(j => j.TransportationType)
                    .Include(j => j.StartingLocation)
                    .Include(j => j.ArrivalLocation)
                    .AsNoTracking();

                var userParams = request.QueryParameters.ToUserParametersDto();
                PagedList<UserDto> users = ApplyUserFilters(request, userParams, ref collection);

                FilterJourneyProperties(request, ref collection);

                ApplySorting(request, ref collection);

                users = await GetUserList(request, collection, userParams, users);

                var journeysAndUsers = await CombineJourneysAndUsers(collection, users);

                var result = PagedList<JourneyDashboardDto>.Create(journeysAndUsers,
                                    request.QueryParameters.PageNumber,
                                    request.QueryParameters.PageSize);

                // Set the cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set(cacheKey, result, cacheEntryOptions);
                return result;
            }

            /// <summary>
            /// Filter using the user specific filters
            /// </summary>
            /// <param name="request"></param>
            /// <param name="userParams"></param>
            /// <param name="collection"></param>
            /// <returns></returns>
            private PagedList<Infrastructure.Models.UserDto> ApplyUserFilters(Query request, UserParametersDto userParams, ref IQueryable<Journey> collection)
            {
                if (!HasUserSearch(request))
                    return new PagedList<Infrastructure.Models.UserDto>();

                var users = _grpcUserClient.GetUsers(userParams);
                var userIds = users.Select(u => u.Id);
                collection = collection.Where(j => userIds.Contains(j.UserId));

                return users;
            }

            /// <summary>
            /// Filter using the journey specific filters
            /// </summary>
            /// <param name="request"></param>
            /// <param name="collection"></param>
            private static void FilterJourneyProperties(Query request, ref IQueryable<Journey> collection)
            {
                if (request.QueryParameters.UserId != null)
                {
                    collection = collection.Where(journey => journey.UserId == request.QueryParameters.UserId);
                }
                if (request.QueryParameters.StartingTimeFrom.HasValue)
                {
                    collection = collection.Where(journey => journey.StartingTime >= request.QueryParameters.StartingTimeFrom.Value);
                }
                if (request.QueryParameters.StartingTimeTo.HasValue)
                {
                    collection = collection.Where(journey => journey.StartingTime <= request.QueryParameters.StartingTimeTo.Value);
                }
                if (request.QueryParameters.MinimumTravelDistance.HasValue)
                {
                    collection = collection.Where(journey => journey.RouteDistance >= request.QueryParameters.MinimumTravelDistance.Value);
                }
                if (request.QueryParameters.MaximumTravelDistance.HasValue)
                {
                    collection = collection.Where(journey => journey.RouteDistance <= request.QueryParameters.MaximumTravelDistance.Value);
                }
            }

            /// <summary>
            /// Apply ordering to the journy query
            /// </summary>
            /// <param name="request"></param>
            /// <param name="collection"></param>
            private void ApplySorting(Query request, ref IQueryable<Journey> collection)
            {
                var sortMapping = new Dictionary<string, Expression<Func<Journey, object>>>
                {
                    { nameof(Journey.StartingTime), journey => journey.StartingTime },
                    { nameof(Journey.ArrivalTime), journey => journey.ArrivalTime },
                    { nameof(Journey.RouteDistance), journey => journey.RouteDistance }
                };

                if (!String.IsNullOrWhiteSpace(request.QueryParameters.SortBy) && sortMapping.TryGetValue(request.QueryParameters.SortBy, out var sortExpression))
                {
                    collection = request.QueryParameters.Descending
                        ? collection.OrderByDescending(sortExpression)
                        : collection.OrderBy(sortExpression);
                }
            }

            /// <summary>
            /// Get the user list from grpc
            /// </summary>
            /// <param name="request"></param>
            /// <param name="collection"></param>
            /// <param name="userParams"></param>
            /// <param name="users"></param>
            /// <returns></returns>
            private async Task<PagedList<Infrastructure.Models.UserDto>> GetUserList(Query request, IQueryable<Journey> collection, UserParametersDto userParams, PagedList<Infrastructure.Models.UserDto> users)
            {
                if (!HasUserSearch(request))
                {
                    userParams.SetFilterByUserId(await collection.Select(j => j.UserId).Distinct().ToListAsync());
                    users = _grpcUserClient.GetUsers(userParams);
                }

                return users;
            }

            /// <summary>
            /// Get the journey data along with the users data
            /// </summary>
            /// <param name="collection"></param>
            /// <param name="users"></param>
            /// <returns></returns>
            private async Task<IEnumerable<JourneyDashboardDto>> CombineJourneysAndUsers(IQueryable<Journey> collection, PagedList<Infrastructure.Models.UserDto> users)
            {
                var journeys = await collection.ToListAsync();

                return from journey in journeys
                       join user in users on journey.UserId equals user.Id
                       select new JourneyDashboardDto
                       {
                           Id = journey.Id,
                           UserId = journey.UserId,
                           TransportationType = journey.TransportationType?.Type,
                           TransportationTypeId = journey.TransportationTypeId,
                           ArrivalLocation = journey.ArrivalLocation?.Name,
                           ArrivalLocationId = journey.ArrivalLocationId,
                           RouteDistance = journey.RouteDistance,
                           StartingTime = journey.StartingTime,
                           ArrivalTime = journey.ArrivalTime,
                           StartingLocation = journey.StartingLocation?.Name,
                           StartingLocationId = journey.StartingLocationId,
                           IsGoalAchieved = journey.IsGoalAchieved,
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           Email = user.Email,
                           DailyGoal = user.DailyGoal
                       };
            }

            private bool HasUserSearch(Query request)
            {
                return request.QueryParameters.MinDailyGoal.HasValue ||
                       request.QueryParameters.MaxDailyGoal.HasValue ||
                       !String.IsNullOrEmpty(request.QueryParameters.FirstName) ||
                       !String.IsNullOrEmpty(request.QueryParameters.LastName) ||
                       !String.IsNullOrEmpty(request.QueryParameters.Email);
            }
        }
    }
}