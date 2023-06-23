using Netcompany.Net.Cqs.Commands;
using RoutePlanning.Domain.BookingRequest;
using RoutePlanning.Domain.Locations;

namespace RoutePlanning.Application.Locations.Commands.UpdateBookingStatus;

public sealed record UpdateBookingStatusCommand(BookingRequest BookingRequest) : ICommand;
