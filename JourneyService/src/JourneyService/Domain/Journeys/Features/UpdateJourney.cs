
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Services;
using JourneyService.Domain.Locations.Services;
using JourneyService.Services;
using MediatR;

namespace JourneyService.Domain.Journeys.Features;
public static class UpdateJourney
{
    public sealed class Command : IRequest
    {
        public readonly Guid Id;
        public readonly PostJourneyDto UpdatedJourneyData;

        public Command(Guid id, PostJourneyDto updatedJourneyData)
        {
            Id = id;
            UpdatedJourneyData = updatedJourneyData;
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly ILocationService _locationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IJourneyRepository journeyRepository, IUnitOfWork unitOfWork, ILocationService locationService, ICurrentUserService currentUserService)
        {
            _journeyRepository = journeyRepository;
            _unitOfWork = unitOfWork;
            _locationService = locationService;
            _currentUserService = currentUserService;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            Journey journeyToUpdate = await _journeyRepository.GetById(request.Id, cancellationToken: cancellationToken);
            request.UpdatedJourneyData.UserId = Guid.Parse(_currentUserService.UserId);
            journeyToUpdate.Update(request.UpdatedJourneyData);

            var existingLocation = await _locationService.GetOrCreateLocationAsync(request.UpdatedJourneyData.StartingLocation, cancellationToken);
            journeyToUpdate.SetStartingLocationId(existingLocation.Id);

            if (!string.IsNullOrWhiteSpace(request.UpdatedJourneyData.ArrivalLocation))
            {
                var existingArrivalLocation = await _locationService.GetOrCreateLocationAsync(request.UpdatedJourneyData.ArrivalLocation, cancellationToken);
                journeyToUpdate.SetArrivalLocationId(existingArrivalLocation.Id);
            }

            _journeyRepository.Update(journeyToUpdate);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}