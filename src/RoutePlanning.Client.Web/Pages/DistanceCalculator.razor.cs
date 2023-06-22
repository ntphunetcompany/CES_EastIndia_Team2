using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RoutePlanning.Application.Locations.Commands.CreateBookingRequest;
using RoutePlanning.Application.Locations.Commands.Distance;
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
        await LogToConsole(Username);
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
            DisplayDistance = await Mediator.Send(new DistanceQuery(SelectedSource.LocationId, SelectedDestination.LocationId), CancellationToken.None);
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
                    DisplayDistance.Value, 100, PackageLength, PackageWidth, PackageHeight, PackageWeight, ShippingDate);

                BookRequestId = await Mediator.Send(bookingRequest, CancellationToken.None);
                await LogToConsole(BookRequestId.ToString());
            }
        }
    }
}
