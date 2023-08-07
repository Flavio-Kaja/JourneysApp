namespace JourneyService.Domain.Journeys.Dtos
{
    public class JourneyDashboardDto : JourneyDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TransportationType { get; set; } = string.Empty;
        public int DailyGoal { get; set; }
    }
}
