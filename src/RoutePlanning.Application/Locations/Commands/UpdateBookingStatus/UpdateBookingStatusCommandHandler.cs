using Netcompany.Net.Cqs.Commands;
using Netcompany.Net.DomainDrivenDesign.Services;
using RoutePlanning.Domain.BookingRequest;

namespace RoutePlanning.Application.Locations.Commands.UpdateBookingStatus;

public sealed class CreateBookingRequestCommandHandler : ICommandHandler<UpdateBookingStatusCommand>
{
    private readonly IRepository<BookingRequest> _bookingRequest;

    public CreateBookingRequestCommandHandler(IRepository<BookingRequest> bookingRequest)
    {
        _bookingRequest = bookingRequest;
    }

    public async Task Handle(UpdateBookingStatusCommand command, CancellationToken cancellationToken)
    {
     
        await _bookingRequest.Update(command.BookingRequest, cancellationToken);
    }
}
