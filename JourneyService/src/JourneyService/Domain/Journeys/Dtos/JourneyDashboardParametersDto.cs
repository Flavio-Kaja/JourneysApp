using JourneyService.Parameters;

namespace JourneyService.Domain.Journeys.Dtos
{
    public class JourneyDashboardParametersDto : BasePaginationParameters
    {

        public Guid? UserId { get; set; }
        public double? MinimumTravelDistance { get; set; }
        public double? MaximumTravelDistance { get; set; }
        public DateTime? StartingTimeFrom { get; set; }
        public DateTime? StartingTimeTo { get; set; }
        public string SortBy { get; set; }
        public bool Descending { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? MinDailyGoal { get; set; }
        public int? MaxDailyGoal { get; set; }

        public string GenerateCacheKey()
        {
            return $"JourneyDashboard-{UserId}-{MinimumTravelDistance}-{MaximumTravelDistance}-{StartingTimeFrom}-{StartingTimeTo}-{SortBy}-{Descending}-{Email}-{FirstName}-{LastName}-{MinDailyGoal}-{MaxDailyGoal}-{PageNumber}-{PageSize}";
        }
    }
}
