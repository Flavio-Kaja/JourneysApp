using JourneyService.Parameters;
using System.Text.Json.Serialization;

namespace JourneyService.Infrastructure.Models
{
    public sealed class UserParametersDto : BasePaginationParameters
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int MinDailyGoal { get; set; } = 0;
        public int MaxDailyGoal { get; set; } = int.MaxValue;
        public string SortBy { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Guid> UserIds { get; set; } = new List<Guid>();
        public bool Descending { get; set; } = false;

        /// <summary>
        /// When filtering by user id ignore pagination filters
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        public void SetFilterByUserId(IEnumerable<Guid> userIds)
        {
            UserIds.AddRange(userIds);
            PageSize = int.MaxValue;
            DefaultPageSize = int.MaxValue;
            PageNumber = 1;
        }
    }
}
