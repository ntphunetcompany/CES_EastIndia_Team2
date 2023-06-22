using Microsoft.EntityFrameworkCore;
using Netcompany.Net.Cqs.Queries;
using RoutePlanning.Domain.BookingRequest;
using RoutePlanning.Domain.Locations;

namespace RoutePlanning.Application.Locations.Queries.GetBookingRequest;

public sealed class GetBookingRequestQueryhandler : IQueryHandler<GetBookingRequestQuery, IReadOnlyList<BookingRequest>>
{
    private readonly IQueryable<BookingRequest> _bookingRequest;

    public GetBookingRequestQueryhandler(IQueryable<BookingRequest> bookingRequest)
    {
        _bookingRequest = bookingRequest;
    }

    public async Task<IReadOnlyList<BookingRequest>> Handle(GetBookingRequestQuery _, CancellationToken cancellationToken)
    {
        return await _bookingRequest
            .ToListAsync(cancellationToken);
    }
}
