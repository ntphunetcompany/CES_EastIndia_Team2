using Netcompany.Net.Cqs.Commands;
using RoutePlanning.Domain.Locations;
using RoutePlanning.Domain.Users;

namespace RoutePlanning.Application.Locations.Commands.CreateBookingRequest;

public sealed record CreateBookingRequestCommand(string Username, string SourceLocationName,
    string DestinationLocationName, int Distance, int Price) : ICommand;
