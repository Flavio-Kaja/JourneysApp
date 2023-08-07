namespace JourneyService.Domain.Journeys.Features;

using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Services;
using Mappings;
using MediatR;

public static class GetJourney
{
    public sealed class Query : IRequest<JourneyDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, JourneyDto>
    {
        private readonly IJourneyRepository _journeyRepository;

        public Handler(IJourneyRepository journeyRepository)
        {
            _journeyRepository = journeyRepository;
        }

        public async Task<JourneyDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _journeyRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return result.ToJourneyDto();
        }
    }
}