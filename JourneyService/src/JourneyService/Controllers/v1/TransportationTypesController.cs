namespace JourneyService.Controllers.v1;

using JourneyService.Domain.TransportationTypes.Features;
using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Ardalis.Specification;
using FluentValidation;

[ApiController]
[Route("api/transportationtypes")]
[ApiVersion("1.0")]
public sealed class TransportationTypesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<PostTransportationTypeDto> _validator;

    public TransportationTypesController(IMediator mediator, IValidator<PostTransportationTypeDto> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    /// <summary>
    /// Creates a new Transportation Type record.
    /// </summary>
    [HttpPost(Name = "AddTransportationType")]
    public async Task<ActionResult<TransportationTypeDto>> AddTransportationType([FromBody] PostTransportationTypeDto transportationTypeForCreation)
    {

        _validator.ValidateAndThrow(transportationTypeForCreation);
        var command = new AddTransportationType.Command(transportationTypeForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetTransportationType",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single TransportationType by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetTransportationType")]
    public async Task<ActionResult<TransportationTypeDto>> GetTransportationType(Guid id)
    {
        var query = new GetTransportationType.Query(id);
        var queryResponse = await _mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Transportation Types.
    /// </summary>
    [HttpGet(Name = "GetTransportationTypes")]
    public async Task<IActionResult> GetTransportationTypes()
    {
        var query = new GetTransportationTypeList.Query();
        var queryResponse = await _mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Updates an existing Transportation Type.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateTransportationType")]
    public async Task<IActionResult> UpdateTransportationType(Guid id, PostTransportationTypeDto transportationType)
    {
        _validator.ValidateAndThrow(transportationType);
        var command = new UpdateTransportationType.Command(id, transportationType);
        await _mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing TransportationType record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteTransportationType")]
    public async Task<ActionResult> DeleteTransportationType(Guid id)
    {
        var command = new DeleteTransportationType.Command(id);
        await _mediator.Send(command);
        return NoContent();
    }

}
