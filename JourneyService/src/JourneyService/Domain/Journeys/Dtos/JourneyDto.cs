namespace JourneyService.Domain.Journeys.Dtos;

public class JourneyDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string StartingLocation { get; set; }
    public Guid StartingLocationId { get; set; }
    public Guid? ArrivalLocationId { get; set; }
    public DateTime? StartingTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public string ArrivalLocation { get; set; }
    public Guid TransportationTypeId { get; set; }
    public double RouteDistance { get; set; }
    public bool IsGoalAchieved { get; set; }
}
