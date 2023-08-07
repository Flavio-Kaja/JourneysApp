namespace JourneyService.Domain.TransportationTypes.DomainEvents;

public sealed class TransportationTypeUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            