namespace JourneyService.IntegrationTests.FeatureTests.Journeys;

using JourneyService.Domain.Journeys.Dtos;
using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.Domain.Journeys.Features;
using FluentAssertions;
using Domain;
using Xunit;
using System.Threading.Tasks;

public class JourneyListQueryTests : TestBase
{

    [Fact]
    public async Task can_get_journey_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeJourneyOne = new FakeJourneyBuilder().Build();
        var fakeJourneyTwo = new FakeJourneyBuilder().Build();
        var queryParameters = new JourneyParametersDto();

        await testingServiceScope.InsertAsync(fakeJourneyOne, fakeJourneyTwo);

        // Act
        var query = new GetJourneyList.Query(queryParameters);
        var journeys = await testingServiceScope.SendAsync(query);

        // Assert
        journeys.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}