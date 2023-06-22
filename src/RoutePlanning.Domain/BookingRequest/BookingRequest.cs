using System.Diagnostics;
using Netcompany.Net.DomainDrivenDesign.Models;
using RoutePlanning.Domain.Locations;
using RoutePlanning.Domain.Users;

namespace RoutePlanning.Domain.BookingRequest;


[DebuggerDisplay("{SourceLocationName} --{Distance}--> {DestinationLocationName}. Booking by {Username} with Price: {Price}")]
public sealed class BookingRequest : AggregateRoot<BookingRequest>
{
    public BookingRequest() // Parameterless constructor
    {
        Username = string.Empty;
        SourceLocationName = string.Empty;
        DestinationLocationName = string.Empty;
    }
    
    public BookingRequest(string username, string sourceLocation,
        string destinationLocation, int distance, int price)
    {
        Username = username;
        SourceLocationName = sourceLocation;
        DestinationLocationName = destinationLocation;
        Distance = distance;
        Price = price;
    }


    
    public string Username { get; set; }
    public string SourceLocationName { get; set; }

    public string DestinationLocationName { get; set; }

    public Distance? Distance { get; set; }
    
    public int Price { get; set; }

    public BookingRequestStatus BookingStatus { get; set; } = BookingRequestStatus.Pending;
    
}
