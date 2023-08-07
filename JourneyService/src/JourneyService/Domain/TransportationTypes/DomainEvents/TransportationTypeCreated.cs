namespace JourneyService.Domain.TransportationTypes.DomainEvents;

public sealed class TransportationTypeCreated : DomainEvent
{
    public TransportationType TransportationType { get; set; } 
}
            