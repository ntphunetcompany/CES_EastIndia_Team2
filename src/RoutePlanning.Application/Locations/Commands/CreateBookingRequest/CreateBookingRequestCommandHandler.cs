﻿using Microsoft.EntityFrameworkCore;
using Netcompany.Net.Cqs.Commands;
using Netcompany.Net.DomainDrivenDesign.Services;
using RoutePlanning.Domain.BookingRequest;

namespace RoutePlanning.Application.Locations.Commands.CreateBookingRequest;


public sealed class CreateBookingRequestCommandHandler : ICommandHandler<CreateBookingRequestCommand>
{
    private readonly IRepository<BookingRequest> _bookingRequest;

    public CreateBookingRequestCommandHandler(IRepository<BookingRequest> bookingRequest)
    {
        _bookingRequest = bookingRequest;
    }

    public async Task Handle(CreateBookingRequestCommand command, CancellationToken cancellationToken)
    {
        var bookingRequest = new BookingRequest(
            command.Username,
            command.SourceLocationName,
            command.DestinationLocationName,
            command.Distance,
            command.Price
        );

        await _bookingRequest.Add(bookingRequest, cancellationToken);
    }
}
