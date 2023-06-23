using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RoutePlanning.Application.Locations.Commands.CreateBookingRequest;
using RoutePlanning.Application.Locations.Commands.CreateTwoWayConnection;
using RoutePlanning.Application.Locations.Queries.Distance;
using RoutePlanning.Application.Locations.Queries.SelectableLocationList;
using RoutePlanning.Client.Web.Authorization;


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

    [HttpPost("Book")]
    public async Task<IActionResult> Book([FromBody][Required] ShipmentDetails shipment)
    {
        if (shipment == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check for extra fields
        if (shipment.ExtraFields != null && shipment.ExtraFields.Any())
        {
            return BadRequest("Additional attributes are not allowed.");
        }

        if (shipment.ShipmentType == "cautious_parcels")
        {
            return StatusCode(403);
        }

        if (shipment.ShipmentType == "live_animals")
        {
            return StatusCode(403);
        }

        IEnumerable<SelectableLocation> locations = await _mediator.Send(new SelectableLocationListQuery(), CancellationToken.None);
        
        var origin = locations.Where(x => x.Name.ToLower() == shipment.Origin.ToLower()).FirstOrDefault();
        if (origin == null)
        {
            return BadRequest("Could not find location: " + shipment.Origin);
        }

        var dest = locations.Where(x => x.Name.ToLower() == shipment.Destination.ToLower()).FirstOrDefault();
        if (dest == null)
        {
            return BadRequest("Could not find location: " + shipment.Destination);
        }

        BookingSearchDto distance = await _mediator.Send(new DistanceQuery(origin.LocationId, dest.LocationId), CancellationToken.None);
        var duration = distance.Distance * 12;

        var cost = IsWinter(shipment.DateOfShipment) ? 8 : 5;
        cost *= distance.Distance;
        cost = getPriceOfProduct(cost, shipment.ShipmentType);

        DateTime expectedArrival = shipment.DateOfShipment.AddMinutes(60 * duration);

        var username = "test";
        var bookingRequest = new CreateBookingRequestCommand(username, shipment.Origin,
        shipment.Destination, distance.Distance, cost,decimal.ToDouble(shipment.Length), decimal.ToDouble(shipment.Width), decimal.ToDouble(shipment.Height), decimal.ToDouble(shipment.Weight), expectedArrival, shipment.DateOfShipment);
        var id = await _mediator.Send(bookingRequest, CancellationToken.None);
       
        return Ok(id);
    }

    [HttpGet("RequestPrice")]
    public async Task<IActionResult> RequestPrice([FromBody][Required] ShipmentDetails shipment)
    {
        if (shipment == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check for extra fields
        if (shipment.ExtraFields != null && shipment.ExtraFields.Any())
        {
            return BadRequest("Additional attributes are not allowed.");
        }

        if (shipment.ShipmentType == "cautious_parcels")
        {
            return StatusCode(403);
        }

        if (shipment.ShipmentType == "live_animals")
        {
            return StatusCode(403);
        }

        IEnumerable<SelectableLocation> locations = await _mediator.Send(new SelectableLocationListQuery(), CancellationToken.None);
        var origin = locations.Where(x => x.Name.ToLower() == shipment.Origin.ToLower()).FirstOrDefault();
        if (origin == null)
        {
            return BadRequest("Could not find location: " + shipment.Origin);
        }

        var dest = locations.Where(x => x.Name.ToLower() == shipment.Destination.ToLower()).FirstOrDefault();
        if (dest == null)
        {
            return BadRequest("Could not find location: " + shipment.Destination);
        }

        var distance = await _mediator.Send(new DistanceQuery(origin.LocationId, dest.LocationId), CancellationToken.None);
        var duration = distance.Distance * 12;

        var cost = IsWinter(shipment.DateOfShipment) ? 8 : 5;
        cost *= distance.Distance;
        cost = getPriceOfProduct(cost, shipment.ShipmentType);

        var output = new RouteDetails();
        output.Origin = shipment.Origin;
        output.Destination = shipment.Destination;
        output.Duration = duration;
        output.Cost = cost;
        return Ok(output);
    }

    public int getPriceOfProduct(int baseCost, string shipmentType)
    {
        switch (shipmentType) {
            case "weapons":
                return (int)(Math.Round(baseCost * 1.2, MidpointRounding.AwayFromZero));
            case "refrigerated_goods":
                return (int)(Math.Round(baseCost * 1.1, MidpointRounding.AwayFromZero));
            case "live_animals":
                return (int)(Math.Round(baseCost * 1.25, MidpointRounding.AwayFromZero));
            default:
                return baseCost;
        }
    }

    public static bool IsWinter(System.DateTime date)
    {
        var month = date.Month;

        return month >= 11 || month <= 4;
    }
}
/*
[Route("api")]
[ApiController]
[Authorize(nameof(TokenRequirement))]
public sealed class SearchController : ControllerBase
{
    private readonly IShortestDistanceService _shortestDistanceService;
    private readonly ICheapestDistanceService _cheapestDistanceService;

    public SearchController(IShortestDistanceService shortestDistanceService, ICheapestDistanceService cheapestDistanceService)
    {
        _shortestDistanceService = shortestDistanceService;
        _cheapestDistanceService = cheapestDistanceService;
    }

    [HttpGet("routes")]
    //public Task<SearchResultDto> Search(SearchRequestDto)
    public SearchResultDto Search(SearchRequestDto searchRequestDto)
    {
        var shortestPath = _shortestDistanceService.GetShortestPath(searchRequestDto.Origin, searchRequestDto.Destination);
        var fastestRoute = SearchResult.GetSearchResult(shortestPath, searchRequestDto);

        var cheapestPath = _cheapestDistanceService.GetCheapestPath(searchRequestDto.Origin, searchRequestDto.Destination);
        var cheapestRoute = SearchResult.GetSearchResult(cheapestPath, searchRequestDto);

        return new SearchResultDto(null, cheapestRoute, fastestRoute);
    }
}
/*
public class SearchRequestDto
{
    public Location Origin { get; set; }
    public Location Destination { get; set; }
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
        Location origin,
        Location destination,
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
    public SearchResult? RecommendedRoute { get; set; }
    public SearchResult? CheapestRoute { get; set; }
    public SearchResult? FastestRoute { get; set; }

    public SearchResultDto(
        SearchResult? recommendedRoute,
        SearchResult? cheapestRoute,
        SearchResult? fastestRoute)
    {
        RecommendedRoute = recommendedRoute;
        CheapestRoute = cheapestRoute;
        FastestRoute = fastestRoute;
    }
}
/*
public class SearchResult
{
    public int NumberOfDays { get; set; }
    public Location Origin { get; set; }
    public Location Destination { get; set; }
    public DateOnly DateDestination { get; set; }
    public decimal Price { get; set; }
    public IEnumerable<Connection>? Waypoints { get; set; }

    public SearchResult(
        int numberOfDays,
        Location origin,
        Location destination,
        DateOnly dateDestination,
        decimal price,
        IEnumerable<Connection>? waypoints)
    {
        NumberOfDays = numberOfDays;
        Origin = origin;
        Destination = destination;
        DateDestination = dateDestination;
        Price = price;
        Waypoints = waypoints;
    }

    public static SearchResult GetSearchResult(IEnumerable<Connection>? shortestPath, SearchRequestDto searchRequestDto)
    {
        var time = shortestPath?.Sum(c => c.Distance);
        var basePrice = shortestPath?.Sum(c => c.Price);
        var isWinter = DateChecker.IsWinter(searchRequestDto.DateOrigin);
        var price = basePrice
            * (isWinter ? 8m : 5m)
            * (searchRequestDto.IsAWeapon ? 1.2m : 1m)
            * (searchRequestDto.IsALiveAnimal ? 1.25m : 1m)
            * (searchRequestDto.IsAWeapon ? 1.1m : 1m);
        return new SearchResult(
            time ?? 0,
            searchRequestDto.Origin,
            searchRequestDto.Destination,
            searchRequestDto.DateOrigin.AddDays(time ?? 0),
            price ?? 0,
            shortestPath
        );
    }
}

public static class DateChecker
{
    public static bool IsWinter(DateOnly date)
    {
        var month = date.Month;

        return month >= 11 || month <= 4;
    }
}
*/
