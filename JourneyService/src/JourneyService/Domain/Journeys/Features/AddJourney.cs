using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Mappings;
using JourneyService.Domain.Journeys.Services;
using JourneyService.Domain.Locations.Services;
using JourneyService.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyService.Domain.Journeys.Features;
public static class AddJourney
{
    public sealed class Command : IRequest<JourneyDto>
    {
        public readonly PostJourneyDto JourneyToAdd;

        public Command(PostJourneyDto journeyToAdd)
        {
            JourneyToAdd = journeyToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, JourneyDto>
    {
        private readonly ILocationService _locationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJourneyRepository _journeyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IJourneyRepository journeyRepository, IUnitOfWork unitOfWork, ILocationService locationService, ICurrentUserService currentUserService)
        {
            _journeyRepository = journeyRepository;
            _unitOfWork = unitOfWork;
            _locationService = locationService;
            _currentUserService = currentUserService;
        }

        public async Task<JourneyDto> Handle(Command request, CancellationToken cancellationToken)
        {
            request.JourneyToAdd.UserId = Guid.Parse(_currentUserService.UserId);
            Journey journey = Journey.Create(request.JourneyToAdd);

            var existingLocation = await _locationService.GetOrCreateLocationAsync(request.JourneyToAdd.StartingLocation, cancellationToken);
            journey.SetStartingLocationId(existingLocation.Id);

            if (!string.IsNullOrWhiteSpace(request.JourneyToAdd.ArrivalLocation))
            {
                var existingArrivalLocation = await _locationService.GetOrCreateLocationAsync(request.JourneyToAdd.ArrivalLocation, cancellationToken);
                journey.SetArrivalLocationId(existingArrivalLocation.Id);
            }

            var journeysMadeToday = _journeyRepository.Query().AsNoTracking().Where(c => c.ArrivalTime > DateTime.Today).Sum(j => j.RouteDistance);
            if (_currentUserService.DailyGoal.HasValue)
            {
                journey.SetGoalAchieved(journeysMadeToday + request.JourneyToAdd.RouteDistance, _currentUserService.DailyGoal.Value);
            }
            await _journeyRepository.Add(journey, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return journey.ToJourneyDto();
        }
    }
}