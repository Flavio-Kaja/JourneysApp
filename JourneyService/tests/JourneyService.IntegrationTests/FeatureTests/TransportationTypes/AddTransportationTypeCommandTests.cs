namespace JourneyService.IntegrationTests.FeatureTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using Domain;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using JourneyService.Domain.TransportationTypes.Features;

public class AddTransportationTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_transportationtype_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeTransportationTypeOne = new FakeTransportationTypeForCreationDto().Generate();

        // Act
        var command = new AddTransportationType.Command(fakeTransportationTypeOne);
        var transportationTypeReturned = await testingServiceScope.SendAsync(command);
        var transportationTypeCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes
            .FirstOrDefaultAsync(t => t.Id == transportationTypeReturned.Id));

        // Assert
        transportationTypeReturned.Type.Should().Be(fakeTransportationTypeOne.Type);

        transportationTypeCreated.Type.Should().Be(fakeTransportationTypeOne.Type);
    }
}