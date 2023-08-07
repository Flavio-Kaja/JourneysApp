namespace JourneyService.Controllers.v1;

using JourneyService.Domain.Journeys.Features;
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using JourneyService.Domain;
using FluentValidation;

[ApiController]
[Route("api/journeys")]
[ApiVersion("1.0")]
public sealed class JourneysController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<PostJourneyDto> _validator;

    public JourneysController(IMediator mediator, IValidator<PostJourneyDto> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }


    /// <summary>
    /// Creates a new Journey record.
    /// </summary>
    [HttpPost(Name = "AddJourney")]
    [Authorize(Policy = Permissions.CanCreateJourney)]
    public async Task<ActionResult<JourneyDto>> AddJourney([FromBody] PostJourneyDto journeyForCreation)
    {
        _validator.ValidateAndThrow(journeyForCreation);
        var command = new AddJourney.Command(journeyForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetJourney",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Journey by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetJourney")]
    public async Task<ActionResult<JourneyDto>> GetJourney(Guid id)
    {
        var query = new GetJourney.Query(id);
        var queryResponse = await _mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Journeys.
    /// </summary>
    [Authorize(Policy = Permissions.CanReadJourney)]
    [HttpGet(Name = "GetJourneys")]
    public async Task<IActionResult> GetJourneys([FromQuery] JourneyParametersDto journeyParametersDto)
    {
        var query = new GetJourneyList.Query(journeyParametersDto);
        var queryResponse = await _mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Updates an entire existing Journey.
    /// </summary>
    [Authorize(Policy = Permissions.CanUpdateJourney)]
    [HttpPut("{id:guid}", Name = "UpdateJourney")]
    public async Task<IActionResult> UpdateJourney(Guid id, PostJourneyDto journey)
    {
        _validator.ValidateAndThrow(journey);
        var command = new UpdateJourney.Command(id, journey);
        await _mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Journey record.
    /// </summary>
    [Authorize(Policy = Permissions.CanDeleteJourney)]
    [HttpDelete("{id:guid}", Name = "DeleteJourney")]
    public async Task<ActionResult> DeleteJourney(Guid id)
    {
        var command = new DeleteJourney.Command(id);
        await _mediator.Send(command);
        return NoContent();
    }

}
