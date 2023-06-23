using Netcompany.Net.Cqs.Commands;
using RoutePlanning.Domain.BookingRequest;
using RoutePlanning.Domain.Locations;
using RoutePlanning.Domain.Users;

namespace RoutePlanning.Application.Locations.Commands.CreateBookingRequest;

public sealed record CreateBookingRequestCommand(string Username, string SourceLocationName,
    string DestinationLocationName, int Distance, double? Price, double? Length, double? Width, double? Height, double? Weight, 
    DateTime? EstimatedDateTime, DateTime DateTime) 
    : ICommand<BookingRequest.EntityId>;
