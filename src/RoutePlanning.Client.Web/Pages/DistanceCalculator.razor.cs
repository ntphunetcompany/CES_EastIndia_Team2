using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RoutePlanning.Application.Locations.Commands.CreateBookingRequest;
using RoutePlanning.Application.Locations.Queries.Distance;
using RoutePlanning.Application.Locations.Queries.GetBookingRequest;
using RoutePlanning.Application.Locations.Queries.SelectableLocationList;
using RoutePlanning.Domain.BookingRequest;

namespace RoutePlanning.Client.Web.Pages;

public sealed partial class DistanceCalculator
{
    private IEnumerable<SelectableLocation>? Locations { get; set; }
    private SelectableLocation? SelectedSource { get; set; }
    private SelectableLocation? SelectedDestination { get; set; }
    private string? DisplaySource { get; set; }
    private string? DisplayDestination { get; set; }
    private int? DisplayDistance { get; set; }

    private string? Username { get; set; }
    
    private BookingRequest.EntityId? BookRequestId { get; set; }

    private IReadOnlyList<BookingRequest> BookingRequest { get; set; } = null!;
    
    private DateTime ShippingDate { get; set; }
    private double? PackageLength { get; set; }
    private double? PackageWidth { get; set; }
    private double? PackageHeight { get; set; }
    private double? PackageWeight { get; set; }
    
    private double? RecommendPrice { get; set; }
    private double? CheapestPrice { get; set; }

    private double? FastestPrice { get; set; }


    private DateTime? EstimatedRecommendShippingDate { get; set; }
    private DateTime? EstimatedCheapestShippingDate { get; set; }
    private DateTime? EstimatedFastestShippingDate { get; set; }
    bool IsRecommendSelected = false;
    bool IsCheapestSelected = false;
    bool IsFastestSelected = false;


    [Inject]
    private IMediator Mediator { get; set; } = default!;

    public async Task LogToConsole(string message)
    {
        await JsRuntime.InvokeVoidAsync("console.log", message);
    }
    
    protected override async Task OnInitializedAsync()
    {
        ShippingDate = DateTime.Today;
        Locations = await Mediator.Send(new SelectableLocationListQuery(), CancellationToken.None);

        BookingRequest = await Mediator.Send(new GetBookingRequestQuery(), CancellationToken.None);
        Username = await JsRuntime.InvokeAsync<string>("localStorageFunctions.getItem",  "username" );
    }

    private async Task CalculateDistance()
    {
        if (Username is null)
        {
            await JsRuntime.InvokeVoidAsync("showWarningFunctions.displayLoginRequire");
            return;
        }
        if (SelectedSource is not null && SelectedDestination is not null)
        {
            DisplaySource = SelectedSource.Name;
            DisplayDestination = SelectedDestination.Name;
            var bookingSearchDto = await Mediator.Send(new DistanceQuery(SelectedSource.LocationId, SelectedDestination.LocationId), CancellationToken.None);
            EstimatedRecommendShippingDate = ShippingDate.AddHours(bookingSearchDto.HoursToShip);
            EstimatedCheapestShippingDate = ShippingDate.AddHours(bookingSearchDto.HoursToShip + bookingSearchDto.HoursToShip*0.3);
            EstimatedFastestShippingDate = ShippingDate.AddHours(bookingSearchDto.HoursToShip - bookingSearchDto.HoursToShip*0.2);
            DisplayDistance = bookingSearchDto.Distance;
            RecommendPrice = bookingSearchDto.Price;
            CheapestPrice = bookingSearchDto.Price * 0.8;
            FastestPrice = bookingSearchDto.Price * 1.3;
        }
    }

    private async Task OnBookingRequest()
    {
        if (SelectedSource is not null && SelectedDestination is not null)
        {
            if (Username is not null && DisplayDistance.HasValue)
            {
                var bookingRequest = new CreateBookingRequestCommand(Username, SelectedSource.Name,
                    SelectedDestination.Name,
                    DisplayDistance.Value, IsRecommendSelected ? RecommendPrice :
                    IsCheapestSelected ? CheapestPrice :
                    IsFastestSelected ? FastestPrice :
                    null, PackageLength, PackageWidth, PackageHeight, PackageWeight, 
                    IsRecommendSelected ? EstimatedRecommendShippingDate :
                    IsCheapestSelected ? EstimatedCheapestShippingDate :
                    IsFastestSelected ? EstimatedFastestShippingDate :
                    null, ShippingDate);

                BookRequestId = await Mediator.Send(bookingRequest, CancellationToken.None);
                await JsRuntime.InvokeVoidAsync("showWarningFunctions.displayRegisterSuccess");

            }
        }
    }
}
