using System.Diagnostics;
using Netcompany.Net.DomainDrivenDesign.Models;
using RoutePlanning.Domain.Locations;
using RoutePlanning.Domain.Users;

namespace RoutePlanning.Domain.BookingRequest;


[DebuggerDisplay("{SourceLocationName} --{Distance}--> {DestinationLocationName}. Booking by {Username} with Price: {Price}")]
public sealed class BookingRequest : AggregateRoot<BookingRequest>
{
    public BookingRequest()
    {
        Username = string.Empty;
        SourceLocationName = string.Empty;
        DestinationLocationName = string.Empty;
    }
    
    public BookingRequest(string username, string sourceLocation,
        string destinationLocation, int distance, double? price, double? length, double? width, double? height, 
        double? weight, DateTime? estimatedDateTime, DateTime dateTime)
    {
        Username = username;
        SourceLocationName = sourceLocation;
        DestinationLocationName = destinationLocation;
        Distance = distance;
        Price = price;
        Length = length;
        Width = width;
        Height = height;
        Weight = weight;
        EstimatedDateTime = estimatedDateTime;
        DateTime = dateTime;
    }
    
    public BookingRequest(string username, string sourceLocation,
        string destinationLocation, int distance, double? price, double? length, double? width, double? height, 
        double? weight, DateTime? estimatedDateTime, DateTime dateTime, BookingRequestStatus bookingRequestStatus)
    {
        Username = username;
        SourceLocationName = sourceLocation;
        DestinationLocationName = destinationLocation;
        Distance = distance;
        Price = price;
        Length = length;
        Width = width;
        Height = height;
        Weight = weight;
        EstimatedDateTime = estimatedDateTime;
        DateTime = dateTime;
        BookingStatus = bookingRequestStatus;
    }


    
    public string Username { get; set; }
    public string SourceLocationName { get; set; }

    public string DestinationLocationName { get; set; }

    public int? Distance { get; set; }
    
    public double? Price { get; set; }
    
    public double? Length { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }

    public DateTime DateTime { get; set; }
    public DateTime? EstimatedDateTime { get; set; }

    public BookingRequestStatus BookingStatus { get; set; } = BookingRequestStatus.Pending;

    public void UpdateBookingStatus(BookingRequestStatus bookingRequestStatus)
    {
        BookingStatus = bookingRequestStatus;
    }
    
}
