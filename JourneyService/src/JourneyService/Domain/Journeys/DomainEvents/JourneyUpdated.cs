namespace JourneyService.Domain.Journeys.DomainEvents;

public sealed class JourneyUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            