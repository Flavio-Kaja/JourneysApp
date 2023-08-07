
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Mappings;
using JourneyService.Domain.Journeys.Services;
using JourneyService.Infrastructure.Grpc;
using JourneyService.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace JourneyService.Domain.Journeys.Features
{
    public static class GetMonthlyRoutes
    {
        public sealed class Query : IRequest<PagedList<MonthlyJourneySummary>>
        {
            public readonly MonthlyRoutesParametersDto QueryParameters;

            public Query(MonthlyRoutesParametersDto queryParameters) => QueryParameters = queryParameters;
        }

        public sealed class Handler : IRequestHandler<Query, PagedList<MonthlyJourneySummary>>
        {
            private readonly IJourneyRepository _journeyRepository;
            private readonly IGrpcUserClient _grpcUserClient;
            private readonly IMemoryCache _cache;

            public Handler(IJourneyRepository journeyRepository, IGrpcUserClient grpcUserClient, IMemoryCache cache)
            {
                _journeyRepository = journeyRepository;
                _grpcUserClient = grpcUserClient;
                _cache = cache;
            }

            public async Task<PagedList<MonthlyJourneySummary>> Handle(Query request, CancellationToken cancellationToken)
            {
                string cacheKey = request.QueryParameters.GenerateCacheKey();

                if (_cache.TryGetValue(cacheKey, out PagedList<MonthlyJourneySummary> cachedResult))
                {
                    return cachedResult;
                }

                var userParams = request.QueryParameters.ToUserParametersDto();
                var collection = ApplyQueryFilters(request.QueryParameters);

                var data = await collection
                    .GroupBy(j => new { j.UserId, j.StartingTime.Year, j.StartingTime.Month })
                    .Select(g => new MonthlyJourneySummary
                    {
                        UserId = g.Key.UserId,
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalDistance = g.Sum(j => j.RouteDistance),
                        JourneyCount = g.Count()
                    })
                    .ToListAsync();

                var userIds = collection.Select(u => u.UserId).Distinct();
                userParams.SetFilterByUserId(userIds);
                var userData = _grpcUserClient.GetUsers(userParams);

                // Combined the journey and user data
                var monthlyData = from journey in data
                                  join user in userData on journey.UserId equals user.Id
                                  select new MonthlyJourneySummary
                                  {
                                      UserId = journey.UserId,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Year = journey.Year,
                                      Month = journey.Month,
                                      TotalDistance = journey.TotalDistance,
                                      JourneyCount = journey.JourneyCount
                                  };

                //sort the data
                Func<MonthlyJourneySummary, object> sortExpression = GetSortExpression(request.QueryParameters.SortBy);
                monthlyData = request.QueryParameters.Descending
                   ? monthlyData.OrderByDescending(sortExpression)
                   : monthlyData.OrderBy(sortExpression);

                var result = PagedList<MonthlyJourneySummary>.Create(monthlyData,
                                    request.QueryParameters.PageNumber,
                                    request.QueryParameters.PageSize);
                return result;
            }

            private IQueryable<Journey> ApplyQueryFilters(MonthlyRoutesParametersDto parameters)
            {
                return _journeyRepository.Query().AsNoTracking()
                    .Where(j =>
                        (!parameters.Year.HasValue || j.StartingTime.Year == parameters.Year) &&
                        (!parameters.UserId.HasValue || j.UserId == parameters.UserId) &&
                        (!parameters.Month.HasValue || j.StartingTime.Month == parameters.Month));
            }

            private Func<MonthlyJourneySummary, object> GetSortExpression(string sortBy)
            {
                if (sortBy == nameof(MonthlyJourneySummary.TotalDistance))
                    return journey => journey.TotalDistance;
                if (sortBy == nameof(MonthlyJourneySummary.JourneyCount))
                    return journey => journey.JourneyCount;
                if (sortBy == nameof(MonthlyJourneySummary.Year))
                    return journey => journey.Year;

                return journey => journey.Year;
            }

        }
    }
}

