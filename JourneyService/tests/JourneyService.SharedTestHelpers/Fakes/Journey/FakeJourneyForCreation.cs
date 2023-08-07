namespace JourneyService.SharedTestHelpers.Fakes.Journey;

using AutoBogus;
using JourneyService.Domain.Journeys;
using JourneyService.Domain.Journeys.Dtos;

public sealed class FakeJourneyForCreation : AutoFaker<PostJourneyDto>
{
    public FakeJourneyForCreation()
    {
    }
}