namespace JourneyService.Domain.Journeys.Features;

using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Services;
using JourneyService.Wrappers;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public static class GetJourneyList
{
    public sealed class Query : IRequest<PagedList<JourneyDto>>
    {
        public readonly JourneyParametersDto QueryParameters;


        public Query(JourneyParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<JourneyDto>>
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IMemoryCache _cache;

        public Handler(IJourneyRepository journeyRepository, IMemoryCache cache)
        {
            _journeyRepository = journeyRepository;
            _cache = cache;
        }

        public async Task<PagedList<JourneyDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            string cacheKey = request.QueryParameters.GenerateCacheKey();

            // Try to get the result from the cache
            if (_cache.TryGetValue(cacheKey, out PagedList<JourneyDto> cachedResult))
            {
                return cachedResult;
            }


            var collection = _journeyRepository.Query().AsNoTracking();

            //filtering 
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

            //ordering
            if (StringComparer.OrdinalIgnoreCase.Equals(request.QueryParameters.SortBy, nameof(Journey.StartingTime)))
            {
                collection = request.QueryParameters.Descending
                    ? collection.OrderByDescending(journey => journey.StartingTime)
                    : collection.OrderBy(journey => journey.StartingTime);
            }

            else if (StringComparer.OrdinalIgnoreCase.Equals(request.QueryParameters.SortBy, nameof(Journey.ArrivalTime)))
            {
                collection = request.QueryParameters.Descending
                    ? collection.OrderByDescending(journey => journey.ArrivalTime)
                    : collection.OrderBy(journey => journey.ArrivalTime);
            }

            else if (StringComparer.OrdinalIgnoreCase.Equals(request.QueryParameters.SortBy, nameof(Journey.RouteDistance)))
            {
                collection = request.QueryParameters.Descending
                    ? collection.OrderByDescending(journey => journey.ArrivalTime)
                    : collection.OrderBy(journey => journey.ArrivalTime);
            }
            var dtoCollection = collection.ToJourneyDtoQueryable();

            var result = await PagedList<JourneyDto>.CreateAsync(dtoCollection,
                            request.QueryParameters.PageNumber,
                            request.QueryParameters.PageSize,
                            cancellationToken);

            // Set the cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                Priority = CacheItemPriority.Normal
            };

            // Save the result in the cache
            _cache.Set(cacheKey, result, cacheEntryOptions);
            return result;
        }
    }
}