namespace JourneyService.Domain.Journeys.Mappings;

using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Infrastructure.Models;
using Riok.Mapperly.Abstractions;
using static Bogus.Person.CardAddress;


[Mapper]
public static partial class JourneyMapper
{

    [MapProperty("ArrivalLocation.Name", "ArrivalLocation")]
    [MapProperty("StartingLocation.Name", "StartingLocation")]
    public static partial JourneyDto ToJourneyDto(this Journey journey);

    [MapperIgnoreTarget(nameof(UserParametersDto.UserIds))]
    public static partial UserParametersDto ToUserParametersDto(this JourneyDashboardParametersDto journey);

    [MapperIgnoreTarget(nameof(UserParametersDto.UserIds))]
    public static partial UserParametersDto ToUserParametersDto(this MonthlyRoutesParametersDto journey);

    [MapProperty("ArrivalLocation.Name", "ArrivalLocation")]
    [MapProperty("StartingLocation.Name", "StartingLocation")]
    public static partial IQueryable<JourneyDto> ToJourneyDtoQueryable(this IQueryable<Journey> journey);
}