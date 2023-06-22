using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutePlanning.Application.Locations.Commands.CreateTwoWayConnection;
using RoutePlanning.Application.Locations.Queries.SelectableLocationList;
using RoutePlanning.Client.Web.Authorization;
using RoutePlanning.Domain.Locations;

namespace RoutePlanning.Client.Web.Api;

[Route("api/[controller]")]
[ApiController]
[Authorize(nameof(TokenRequirement))]
public sealed class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("[action]")]
    public Task<string> HelloWorld()
    {
        return Task.FromResult("Hello World!");
    }

    [HttpPost("[action]")]
    public async Task AddTwoWayConnection(CreateTwoWayConnectionCommand command)
    {
        await _mediator.Send(command);
    }
}

[Route("api")]
[ApiController]
[Authorize(nameof(TokenRequirement))]
public sealed class SearchController : ControllerBase
{
    //private readonly IMediator _mediator;

    //public SearchController(IMediator mediator)
    //{
    //    _mediator = mediator;
    //}

    [HttpGet("routes")]
    //public Task<SearchResultDto> Search(SearchRequestDto)
    public SearchResultDto Search(SearchRequestDto searchRequestDto)
    {
        var locEntId1 = new Location.EntityId();
        var locEntId2 = new Location.EntityId();

        return new SearchResultDto(
            1,
            searchRequestDto.Origin,
            searchRequestDto.Destination,
            new DateOnly(),
            0,
            new List<SelectableLocation>() { new SelectableLocation(locEntId1, "TestName0"), new SelectableLocation(locEntId2, "TestName1") }
            );
    }
}

// TODO: Should be moved
public class SearchRequestDto
{
    public SelectableLocation Origin { get; set; }
    public SelectableLocation Destination { get; set; }
    public DateOnly DateOrigin { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public float Width { get; set; }
    public float Depth { get; set; }
    public bool IsALiveAnimal { get; set; }
    public bool IsAWeapon { get; set; }
    public bool IsRefrigeratedGood { get; set; }
    public bool IsCautious { get; set; }
    public bool IsRecorded { get; set; }

    public SearchRequestDto(
        SelectableLocation origin,
        SelectableLocation destination,
        DateOnly dateOrigin,
        float weight,
        float height,
        float width,
        float depth,
        bool isALiveAnimal,
        bool isAWeapon,
        bool isRefrigeratedGood,
        bool isCautious,
        bool isRecorded)
    {
        Origin = origin;
        Destination = destination;
        DateOrigin = dateOrigin;
        Weight = weight;
        Height = height;
        Width = width;
        Depth = depth;
        IsALiveAnimal = isALiveAnimal;
        IsAWeapon = isAWeapon;
        IsRefrigeratedGood = isRefrigeratedGood;
        IsCautious = isCautious;
        IsRecorded = isRecorded;
    }
}

public class SearchResultDto
{
    public int NumberOfDays { get; set; }
    public SelectableLocation Origin { get; set; }
    public SelectableLocation Destination { get; set; }
    public DateOnly DateDestination { get; set; }
    public decimal Price { get; set; }
    public List<SelectableLocation> Waypoints { get; set; }

    public SearchResultDto(
        int numberOfDays,
        SelectableLocation origin,
        SelectableLocation destination,
        DateOnly dateDestination,
        decimal price,
        List<SelectableLocation> waypoints)
    {
        NumberOfDays = numberOfDays;
        Origin = origin;
        Destination = destination;
        DateDestination = dateDestination;
        Price = price;
        Waypoints = waypoints;
    }
}
