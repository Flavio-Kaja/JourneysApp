using JourneyService.Domain.Journeys;
using JourneyService.Domain.Locations.DomainEvents;
using JourneyService.Domain.Locations.Dtos;

namespace JourneyService.Domain.Locations;
public class Location : BaseEntity
{
    public string Name { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public ICollection<Journey> Journeys { get; set; }

    public static Location Create(LocationForCreationDto locationForCreation)
    {
        Location newLocation = new()
        {
            Name = locationForCreation.Name,
            Latitude = locationForCreation.Latitude,
            Longitude = locationForCreation.Longitude
        };

        newLocation.QueueDomainEvent(new LocationCreated() { Location = newLocation });

        return newLocation;
    }


    public static Location Create(double latitude, double longitude, string name)
    {
        Location newLocation = new()
        {
            Name = name,
            Latitude = latitude,
            Longitude = longitude
        };

        newLocation.QueueDomainEvent(new LocationCreated() { Location = newLocation });

        return newLocation;
    }
    public Location Update(LocationForCreationDto locationForUpdate)
    {
        Name = locationForUpdate.Name;
        Latitude = locationForUpdate.Latitude;
        Longitude = locationForUpdate.Longitude;

        QueueDomainEvent(new LocationUpdated() { Id = Id });
        return this;
    }

    protected Location() { } // For EF + Mocking
}