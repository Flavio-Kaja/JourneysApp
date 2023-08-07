namespace JourneyService.Domain.Locations.DomainEvents;

public sealed class LocationCreated : DomainEvent
{
    public Location Location { get; set; } 
}
            