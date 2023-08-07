using FluentValidation;
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.TransportationTypes.Services;
using Microsoft.EntityFrameworkCore;

namespace JourneyService.Domain.TransportationTypes.Dtos;

/// <summary>
/// Dto for submiting create or update requests for transportation types
/// </summary>
public sealed class PostTransportationTypeDto
{
    public string Type { get; set; }
}

/// <summary>
/// Validatior for post transportation types
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator&lt;JourneyService.Domain.TransportationTypes.Dtos.PostTransportationTypeDto&gt;" />
public class TransportationTypeDtoValidator : AbstractValidator<PostTransportationTypeDto>
{
    public TransportationTypeDtoValidator(ITransportationTypeRepository transportationTypeRepository)
    {
        RuleFor(j => j.Type)
            .NotEmpty().WithMessage("Please enter the transportation type")
            .Must(j => !transportationTypeRepository.Query().AsNoTracking().Any(t => t.Type.ToLower() == j.ToLower()))
            .WithMessage("A transportation type with this name already exists.");
    }
}