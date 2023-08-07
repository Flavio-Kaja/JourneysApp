namespace JourneyService.SharedTestHelpers.Fakes.Journey;

using JourneyService.Domain.Journeys;
using JourneyService.Domain.Journeys.Dtos;

public class FakeJourneyBuilder
{
    private PostJourneyDto _creationData = new FakeJourneyForCreation().Generate();

    public FakeJourneyBuilder WithModel(PostJourneyDto model)
    {
        _creationData = model;
        return this;
    }

    public FakeJourneyBuilder WithUserId(Guid userId)
    {
        _creationData.UserId = userId;
        return this;
    }

    public FakeJourneyBuilder WithStartingTime(DateTime startingTime)
    {
        _creationData.StartingTime = startingTime;
        return this;
    }

    public FakeJourneyBuilder WithArrivalTime(DateTime arrivalTime)
    {
        _creationData.ArrivalTime = arrivalTime;
        return this;
    }

    public FakeJourneyBuilder WithTransportationTypeId(Guid transportationTypeId)
    {
        _creationData.TransportationTypeId = transportationTypeId;
        return this;
    }

    public FakeJourneyBuilder WithRouteDistance(double routeDistance)
    {
        _creationData.RouteDistance = routeDistance;
        return this;
    }



    public Journey Build()
    {
        var result = Journey.Create(_creationData);
        return result;
    }
}