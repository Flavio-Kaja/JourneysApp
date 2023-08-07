namespace JourneyService.SharedTestHelpers.Fakes.Location;

using JourneyService.Domain.Locations;
using JourneyService.Domain.Locations.Dtos;

public class FakeLocationBuilder
{
    private LocationForCreationDto _creationData = new FakeLocationForCreation().Generate();

    public FakeLocationBuilder WithModel(LocationForCreationDto model)
    {
        _creationData = model;
        return this;
    }

    public FakeLocationBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }

    public FakeLocationBuilder WithLatitude(double latitude)
    {
        _creationData.Latitude = latitude;
        return this;
    }

    public FakeLocationBuilder WithLongitude(double longitude)
    {
        _creationData.Longitude = longitude;
        return this;
    }

    public Location Build()
    {
        var result = Location.Create(_creationData);
        return result;
    }
}