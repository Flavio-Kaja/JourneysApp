namespace JourneyService.Domain.Locations.DomainEvents;

public sealed class LocationUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            