using Netcompany.Net.Cqs.Queries;
using RoutePlanning.Domain.BookingRequest;

namespace RoutePlanning.Application.Locations.Queries.GetBookingRequest;

public sealed record GetBookingRequestQuery : IQuery<IReadOnlyList<BookingRequest>>;
