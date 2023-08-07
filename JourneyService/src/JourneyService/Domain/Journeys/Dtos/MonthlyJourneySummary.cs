namespace JourneyService.Domain.Journeys.Dtos
{
    public class MonthlyJourneySummary
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double TotalDistance { get; set; }
        public int JourneyCount { get; set; }

    }
}
