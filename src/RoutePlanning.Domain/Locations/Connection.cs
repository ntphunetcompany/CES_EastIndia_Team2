using System.Diagnostics;
using Netcompany.Net.DomainDrivenDesign.Models;

namespace RoutePlanning.Domain.Locations;

[DebuggerDisplay("{Source} --{Distance}--> {Destination}")]
public sealed class Connection : Entity<Connection>
{
    public Connection(Location source, Location destination, Distance distance, decimal price = 0)
    {
        Source = source;
        Destination = destination;
        Distance = distance;
        Price = price;
    }

    private Connection()
    {
        Source = null!;
        Destination = null!;
        Distance = null!;
        Price = 0;
    }

    public Location Source { get; private set; }

    public Location Destination { get; private set; }

    public Distance Distance { get; private set; }

    public decimal Price { get; private set; }
}
