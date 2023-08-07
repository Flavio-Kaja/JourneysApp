namespace JourneyService.SharedTestHelpers.Fakes.Location;

using AutoBogus;
using JourneyService.Domain.Locations;
using JourneyService.Domain.Locations.Dtos;

public sealed class FakeLocationForUpdate : AutoFaker<LocationForCreationDto>
{
    public FakeLocationForUpdate()
    {
    }
}