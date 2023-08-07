using Microsoft.AspNetCore.Mvc;

using JourneyService.Domain.Journeys.Features;
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using JourneyService.Domain;

namespace JourneyService.Controllers.v1;

[ApiController]
[Route("api/admin")]
[ApiVersion("1.0")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }


    /// <summary>
    /// Gets the journeys dashboard.
    /// </summary>
    [Authorize(Policy = Permissions.CanFilterJourneys)]
    [HttpGet("JourneysDashboard", Name = "GetJourneysDashboard")]
    public async Task<IActionResult> GetJourneysDashboard([FromQuery] JourneyDashboardParametersDto journeyParametersDto)
    {
        var query = new GetJourneyDashboard.Query(journeyParametersDto);
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
    /// Gets the monthly route distance.
    /// </summary>
    /// <param name="journeyParametersDto">The filtering parameters.</param>
    /// <returns></returns>
    [Authorize(Policy = Permissions.CanViewMonthlyRouteDistance)]
    [HttpGet("MonthlyRouteDistance", Name = "GetMonthlyRouteDistance")]
    public async Task<IActionResult> GetMonthlyRouteDistance([FromQuery] MonthlyRoutesParametersDto journeyParametersDto)
    {
        var query = new GetMonthlyRoutes.Query(journeyParametersDto);
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
}

