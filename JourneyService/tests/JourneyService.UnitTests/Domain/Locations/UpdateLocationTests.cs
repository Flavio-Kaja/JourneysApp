namespace JourneyService.UnitTests.Domain.Locations;

using JourneyService.SharedTestHelpers.Fakes.Location;
using JourneyService.Domain.Locations;
using JourneyService.Domain.Locations.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

public class UpdateLocationTests
{
    private readonly Faker _faker;

    public UpdateLocationTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_location()
    {
        // Arrange
        var fakeLocation = new FakeLocationBuilder().Build();
        var updatedLocation = new FakeLocationForUpdate().Generate();
        
        // Act
        fakeLocation.Update(updatedLocation);

        // Assert
        fakeLocation.Name.Should().Be(updatedLocation.Name);
        fakeLocation.Latitude.Should().Be(updatedLocation.Latitude);
        fakeLocation.Longitude.Should().Be(updatedLocation.Longitude);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeLocation = new FakeLocationBuilder().Build();
        var updatedLocation = new FakeLocationForUpdate().Generate();
        fakeLocation.DomainEvents.Clear();
        
        // Act
        fakeLocation.Update(updatedLocation);

        // Assert
        fakeLocation.DomainEvents.Count.Should().Be(1);
        fakeLocation.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(LocationUpdated));
    }
}