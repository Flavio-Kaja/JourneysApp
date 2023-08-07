using JourneyService.Parameters;

namespace JourneyService.Domain.Journeys.Dtos
{
    public class MonthlyRoutesParametersDto : BasePaginationParameters
    {
        public Guid? UserId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string SortBy { get; set; }
        public bool Descending { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? MinDailyGoal { get; set; }
        public int? MaxDailyGoal { get; set; }

        public string GenerateCacheKey()
        {
            return $"Journey-MonthlyRoutes-{UserId}-{Month}-{Year}-{SortBy}-{Descending}-{Email}-{FirstName}-{LastName}-{MinDailyGoal}-{MaxDailyGoal}-{PageNumber}-{PageSize}";
        }
    }
}
