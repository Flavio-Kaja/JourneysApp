namespace JourneyService.FunctionalTests.FunctionalTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class DeleteTransportationTypeTests : TestBase
{
    [Fact]
    public async Task delete_transportationtype_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeTransportationType = new FakeTransportationTypeBuilder().Build();
        await InsertAsync(fakeTransportationType);

        // Act
        var route = ApiRoutes.TransportationTypes.Delete(fakeTransportationType.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}