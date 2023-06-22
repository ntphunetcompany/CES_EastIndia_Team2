using Microsoft.EntityFrameworkCore;
using Netcompany.Net.Cqs.Commands;
using Netcompany.Net.DomainDrivenDesign.Services;
using RoutePlanning.Domain.BookingRequest;

namespace RoutePlanning.Application.Locations.Commands.CreateBookingRequest;


public sealed class CreateBookingRequestCommandHandler : ICommandHandler<CreateBookingRequestCommand, BookingRequest.EntityId>
{
    private readonly IRepository<BookingRequest> _bookingRequest;

    public CreateBookingRequestCommandHandler(IRepository<BookingRequest> bookingRequest)
    {
        _bookingRequest = bookingRequest;
    }

    public async Task<BookingRequest.EntityId> Handle(CreateBookingRequestCommand command, CancellationToken cancellationToken)
    {
        var bookingRequest = new BookingRequest(
            command.Username,
            command.SourceLocationName,
            command.DestinationLocationName,
            command.Distance,
            command.Price,
            command.Length,
            command.Width,
            command.Height,
            command.Weight,
            command.DateTime
        );
        await _bookingRequest.Add(bookingRequest, cancellationToken);
        return bookingRequest.Id;
    }
}
