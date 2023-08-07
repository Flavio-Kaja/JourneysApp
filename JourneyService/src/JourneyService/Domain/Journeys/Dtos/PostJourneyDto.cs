using FluentValidation;
using System.Text.Json.Serialization;

namespace JourneyService.Domain.Journeys.Dtos;

public sealed class PostJourneyDto
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public string StartingLocation { get; set; }
    public string ArrivalLocation { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public Guid TransportationTypeId { get; set; }
    public double RouteDistance { get; set; }
}
public class PostJourneyDtoValidator : AbstractValidator<PostJourneyDto>
{
    public PostJourneyDtoValidator()
    {
        RuleFor(j => j.StartingLocation)
            .Must(j => !String.IsNullOrWhiteSpace(j))
            .WithMessage("Starting location is required.");

        RuleFor(j => j.StartingTime)
            .NotNull()
            .WithMessage("Starting time is required.");

        RuleFor(x => x.RouteDistance)
        .NotEmpty()
        .WithMessage("Route distance is required")
        .GreaterThan(0)
        .WithMessage("Route distance must be greater than 0");

        RuleFor(j => j.TransportationTypeId)
            .NotNull()
            .WithMessage("TransportationTypeId is required.");

        RuleFor(j => j.RouteDistance)
            .NotNull()
            .WithMessage("RouteDistance is required.");
    }
}