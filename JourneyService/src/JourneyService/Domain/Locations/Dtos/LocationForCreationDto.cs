namespace JourneyService.Domain.Locations.Dtos;

public sealed class LocationForCreationDto
{
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
