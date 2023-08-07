namespace JourneyService.UnitTests.Domain.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.Domain.TransportationTypes;
using JourneyService.Domain.TransportationTypes.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

public class CreateTransportationTypeTests
{
    private readonly Faker _faker;

    public CreateTransportationTypeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_transportationType()
    {
        // Arrange
        var transportationTypeToCreate = new FakeTransportationTypeForCreation().Generate();
        
        // Act
        var fakeTransportationType = TransportationType.Create(transportationTypeToCreate);

        // Assert
        fakeTransportationType.Type.Should().Be(transportationTypeToCreate.Type);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var transportationTypeToCreate = new FakeTransportationTypeForCreation().Generate();
        
        // Act
        var fakeTransportationType = TransportationType.Create(transportationTypeToCreate);

        // Assert
        fakeTransportationType.DomainEvents.Count.Should().Be(1);
        fakeTransportationType.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TransportationTypeCreated));
    }
}