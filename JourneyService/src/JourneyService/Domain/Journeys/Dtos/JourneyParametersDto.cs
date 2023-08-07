namespace JourneyService.Domain.Journeys.Dtos;

using JourneyService.Parameters;

public sealed class JourneyParametersDto : BasePaginationParameters
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public Guid? UserId { get; set; }
    /// <summary>
    /// Gets or sets the minimum travel distance.
    /// </summary>
    public double? MinimumTravelDistance { get; set; }
    /// <summary>
    /// Gets or sets the maximum travel distance.
    /// </summary>
    public double? MaximumTravelDistance { get; set; }
    /// <summary>
    /// Gets or sets the starting time from.
    /// </summary>
    public DateTime? StartingTimeFrom { get; set; }
    /// <summary>
    /// Gets or sets the starting time to.
    /// </summary>
    public DateTime? StartingTimeTo { get; set; }
    /// <summary>
    /// Gets or sets the sort by property.
    /// </summary>
    public string SortBy { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="JourneyParametersDto"/> is descending.
    /// </summary>
    public bool Descending { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance has achieved goal.
    /// </summary>
    public bool HasAchievedGoal { get; set; }

    /// <summary>
    /// Generates the cache key.
    /// </summary>
    /// <returns></returns>
    public string GenerateCacheKey()
    {
        return $"JourneyList-{UserId}-{MinimumTravelDistance}-{MaximumTravelDistance}-{StartingTimeFrom}-{StartingTimeTo}-{SortBy}-{Descending}-{HasAchievedGoal}-{PageNumber}-{PageSize}";
    }
}
