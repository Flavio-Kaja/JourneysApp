namespace JourneyService.FunctionalTests.FunctionalTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class UpdateTransportationTypeRecordTests : TestBase
{
    [Fact]
    public async Task put_transportationtype_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeTransportationType = new FakeTransportationTypeBuilder().Build();
        var updatedTransportationTypeDto = new FakeTransportationTypeForCreationDto().Generate();
        await InsertAsync(fakeTransportationType);

        // Act
        var route = ApiRoutes.TransportationTypes.Put(fakeTransportationType.Id);
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedTransportationTypeDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}