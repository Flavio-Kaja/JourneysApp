namespace JourneyService.Domain.Journeys.DomainEvents;

public sealed class JourneyCreated : DomainEvent
{
    public Journey Journey { get; set; } 
}
            