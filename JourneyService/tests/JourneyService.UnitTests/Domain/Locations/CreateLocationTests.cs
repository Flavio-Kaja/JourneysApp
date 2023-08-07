namespace JourneyService.UnitTests.Domain.Locations;

using JourneyService.SharedTestHelpers.Fakes.Location;
using JourneyService.Domain.Locations;
using JourneyService.Domain.Locations.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

public class CreateLocationTests
{
    private readonly Faker _faker;

    public CreateLocationTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_location()
    {
        // Arrange
        var locationToCreate = new FakeLocationForCreation().Generate();
        
        // Act
        var fakeLocation = Location.Create(locationToCreate);

        // Assert
        fakeLocation.Name.Should().Be(locationToCreate.Name);
        fakeLocation.Latitude.Should().Be(locationToCreate.Latitude);
        fakeLocation.Longitude.Should().Be(locationToCreate.Longitude);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var locationToCreate = new FakeLocationForCreation().Generate();
        
        // Act
        var fakeLocation = Location.Create(locationToCreate);

        // Assert
        fakeLocation.DomainEvents.Count.Should().Be(1);
        fakeLocation.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(LocationCreated));
    }
}