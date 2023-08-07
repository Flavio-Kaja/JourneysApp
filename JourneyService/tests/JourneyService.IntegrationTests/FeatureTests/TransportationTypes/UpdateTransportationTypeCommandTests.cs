namespace JourneyService.IntegrationTests.FeatureTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.Domain.TransportationTypes.Features;
using Domain;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;

public class UpdateTransportationTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_transportationtype_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeTransportationTypeOne = new FakeTransportationTypeBuilder().Build();
        var updatedTransportationTypeDto = new FakeTransportationTypeForCreationDto().Generate();
        await testingServiceScope.InsertAsync(fakeTransportationTypeOne);

        var transportationType = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes
            .FirstOrDefaultAsync(t => t.Id == fakeTransportationTypeOne.Id));

        // Act
        var command = new UpdateTransportationType.Command(transportationType.Id, updatedTransportationTypeDto);
        await testingServiceScope.SendAsync(command);
        var updatedTransportationType = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes.FirstOrDefaultAsync(t => t.Id == transportationType.Id));

        // Assert
        updatedTransportationType.Type.Should().Be(updatedTransportationTypeDto.Type);
    }
}