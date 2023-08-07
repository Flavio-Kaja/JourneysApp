namespace JourneyService.UnitTests.Domain.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.Domain.TransportationTypes;
using JourneyService.Domain.TransportationTypes.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

public class UpdateTransportationTypeTests
{
    private readonly Faker _faker;

    public UpdateTransportationTypeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_transportationType()
    {
        // Arrange
        var fakeTransportationType = new FakeTransportationTypeBuilder().Build();
        var updatedTransportationType = new FakeTransportationTypeForUpdate().Generate();
        
        // Act
        fakeTransportationType.Update(updatedTransportationType);

        // Assert
        fakeTransportationType.Type.Should().Be(updatedTransportationType.Type);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeTransportationType = new FakeTransportationTypeBuilder().Build();
        var updatedTransportationType = new FakeTransportationTypeForUpdate().Generate();
        fakeTransportationType.DomainEvents.Clear();
        
        // Act
        fakeTransportationType.Update(updatedTransportationType);

        // Assert
        fakeTransportationType.DomainEvents.Count.Should().Be(1);
        fakeTransportationType.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TransportationTypeUpdated));
    }
}