using JourneyService.Domain.Journeys.DomainEvents;
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Locations;
using JourneyService.Domain.TransportationTypes;

namespace JourneyService.Domain.Journeys;

/// <summary>Gets or sets the journey entity</summary>
/// <seealso cref="JourneyService.Domain.BaseEntity" />
public class Journey : BaseEntity
{
    /// <summary>Gets the user identifier.</summary>
    /// <value>The user identifier.</value>
    public Guid UserId { get; private set; }

    /// <summary>Gets the starting location identifier.</summary>
    /// <value>The starting location identifier.</value>
    public Guid StartingLocationId { get; private set; }

    /// <summary>Gets the starting location.</summary>
    /// <value>The starting location.</value>
    public Location StartingLocation { get; private set; }

    /// <summary>Gets the arrival location identifier.</summary>
    /// <value>The arrival location identifier.</value>
    public Guid ArrivalLocationId { get; private set; }

    /// <summary>Gets the arrival location.</summary>
    /// <value>The arrival location.</value>
    public Location ArrivalLocation { get; private set; }

    /// <summary>Gets the starting time.</summary>
    /// <value>The starting time.</value>
    public DateTime StartingTime { get; private set; }

    /// <summary>Gets the arrival time.</summary>
    /// <value>The arrival time.</value>
    public DateTime? ArrivalTime { get; private set; }

    /// <summary>Gets the transportation type identifier.</summary>
    /// <value>The transportation type identifier.</value>
    public Guid TransportationTypeId { get; private set; }

    /// <summary>Gets the type of the transportation.</summary>
    /// <value>The type of the transportation.</value>
    public virtual TransportationType TransportationType { get; private set; }

    /// <summary>Gets the route distance.</summary>
    /// <value>The route distance.</value>
    public double RouteDistance { get; private set; }

    /// <summary>Gets a value indicating whether this instance is goal achieved.</summary>
    /// <value>
    ///   <c>true</c> if this instance is goal achieved; otherwise, <c>false</c>.</value>
    public bool IsGoalAchieved { get; private set; }

    /// <summary>Sets the goal achieved.</summary>
    /// <param name="distanceTraveledToday">The distance traveled today.</param>
    /// <param name="dailyGoal">The daily goal.</param>
    public void SetGoalAchieved(double distanceTraveledToday, int dailyGoal)
    {
        IsGoalAchieved = distanceTraveledToday > dailyGoal;
    }

    /// <summary>Sets the starting location identifier.</summary>
    /// <param name="locationId">The location identifier.</param>
    public void SetStartingLocationId(Guid locationId)
    {
        StartingLocationId = locationId;
    }

    /// <summary>Sets the arrival location identifier.</summary>
    /// <param name="locationId">The location identifier.</param>
    public void SetArrivalLocationId(Guid locationId)
    {
        ArrivalLocationId = locationId;
    }

    /// <summary>Creates the specified journey for creation.</summary>
    /// <param name="journeyForCreation">The journey for creation.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static Journey Create(PostJourneyDto journeyForCreation)
    {
        Journey newJourney = new()
        {
            UserId = journeyForCreation.UserId,
            StartingTime = journeyForCreation.StartingTime,
            ArrivalTime = journeyForCreation.ArrivalTime,
            TransportationTypeId = journeyForCreation.TransportationTypeId,
            RouteDistance = journeyForCreation.RouteDistance
        };

        newJourney.QueueDomainEvent(new JourneyCreated() { Journey = newJourney });

        return newJourney;
    }

    /// <summary>Updates the specified journey for update.</summary>
    /// <param name="journeyForUpdate">The journey for update.</param>
    public Journey Update(PostJourneyDto journeyForUpdate)
    {
        UserId = journeyForUpdate.UserId;
        StartingTime = journeyForUpdate.StartingTime;
        ArrivalTime = journeyForUpdate.ArrivalTime;
        TransportationTypeId = journeyForUpdate.TransportationTypeId;
        RouteDistance = journeyForUpdate.RouteDistance;
        QueueDomainEvent(new JourneyUpdated() { Id = Id });
        return this;
    }

    /// <summary>Initializes a new instance of the <see cref="Journey" /> class.</summary>
    protected Journey() { } // For EF + Mocking
}