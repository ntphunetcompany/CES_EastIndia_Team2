using Microsoft.EntityFrameworkCore;

namespace RoutePlanning.Domain.Locations.Services;

public sealed class ShortestDistanceService : IShortestDistanceService
{
    private readonly IQueryable<Location> _locations;

    public ShortestDistanceService(IQueryable<Location> locations)
    {
        _locations = locations;
    }

    public IEnumerable<Connection> GetShortestPath(Location source, Location target)
    {
        var locations = _locations.Include(l => l.Connections).ThenInclude(c => c.Destination);

        return CalculateShortestPath(locations, source, target);
    }

    public int CalculateShortestDistance(Location source, Location target)
    {
        var locations = _locations.Include(l => l.Connections).ThenInclude(c => c.Destination);

        var path = CalculateShortestPath(locations, source, target);

        return path.Sum(c => c.Distance);
    }

    /// <summary>
    /// An implementation of the Dijkstra's shortest path algorithm
    /// </summary>
    private static IEnumerable<Connection> CalculateShortestPath(IEnumerable<Location> locations, Location start, Location end)
    {
        var shortestConnections = CalculateShortestConnections(locations, start, end);

        var path = ConstructShortestPath(start, end, shortestConnections);

        return path;
    }

    /// <summary>
    /// An implementation of the Dijkstra's algorithm that computes the shortest connections to all locations until the end location is reached
    /// </summary>
    private static Dictionary<Location, (Connection? SourceConnection, int Distance)> CalculateShortestConnections(IEnumerable<Location> locations, Location start, Location end)
    {
        var shortestConnections = new Dictionary<Location, (Connection? SourceConnection, int Distance)>();
        var unvisitedLocations = locations.ToHashSet();

        foreach (var location in unvisitedLocations)
        {
            shortestConnections[location] = (SourceConnection: null, Distance: int.MaxValue);
        }

        shortestConnections[start] = (SourceConnection: null, Distance: 0);

        while (unvisitedLocations.Count > 0)
        {
            var location = unvisitedLocations.OrderBy(location => shortestConnections[location].Distance).First();

            if (location == end)
            {
                break;
            }

            foreach (var connection in location.Connections)
            {
                UpdateShortestConnections(shortestConnections, location, connection);
            }

            unvisitedLocations.Remove(location);
        }

        return shortestConnections;
    }

    private static void UpdateShortestConnections(Dictionary<Location, (Connection? SourceConnection, int Distance)> shortestConnections, Location location, Connection connection)
    {
        var distance = shortestConnections[location].Distance + connection.Distance;

        if (distance < shortestConnections[connection.Destination].Distance)
        {
            shortestConnections[connection.Destination] = (SourceConnection: connection, Distance: distance);
        }
    }

    /// <summary>
    /// The shortest path is constructed by backtracking the Dijkstra connection data from the end location
    /// </summary>
    private static IEnumerable<Connection> ConstructShortestPath(Location start, Location end, Dictionary<Location, (Connection? SourceConnection, int Distance)> sourceConnections)
    {
        var path = new List<Connection>();
        var location = end;

        while (location != start)
        {
            var shortestConnection = sourceConnections[location].SourceConnection!;
            path.Add(shortestConnection);
            location = shortestConnection.Source;
        }

        path.Reverse();

        return path;
    }
}

public sealed class CheapestDistanceService : ICheapestDistanceService
{
    private readonly IQueryable<Location> _locations;

    public CheapestDistanceService(IQueryable<Location> locations)
    {
        _locations = locations;
    }

    public IEnumerable<Connection> GetCheapestPath(Location source, Location target)
    {
        var locations = _locations.Include(l => l.Connections).ThenInclude(c => c.Destination);

        return CalculateCheapestPath(locations, source, target);
    }

    public decimal CalculateCheapestDistance(Location source, Location target)
    {
        var locations = _locations.Include(l => l.Connections).ThenInclude(c => c.Destination);

        var path = CalculateCheapestPath(locations, source, target);

        return path.Sum(c => c.Price);
    }

    /// <summary>
    /// An implementation of the Dijkstra's shortest path algorithm
    /// </summary>
    private static IEnumerable<Connection> CalculateCheapestPath(IEnumerable<Location> locations, Location start, Location end)
    {
        var shortestConnections = CalculateCheapestConnections(locations, start, end);

        var path = ConstructCheapestPath(start, end, shortestConnections);

        return path;
    }

    /// <summary>
    /// An implementation of the Dijkstra's algorithm that computes the shortest connections to all locations until the end location is reached
    /// </summary>
    private static Dictionary<Location, (Connection? SourceConnection, decimal Price)> CalculateCheapestConnections(IEnumerable<Location> locations, Location start, Location end)
    {
        var cheapestConnections = new Dictionary<Location, (Connection? SourceConnection, decimal Price)>();
        var unvisitedLocations = locations.ToHashSet();

        foreach (var location in unvisitedLocations)
        {
            cheapestConnections[location] = (SourceConnection: null, Price: decimal.MaxValue);
        }

        cheapestConnections[start] = (SourceConnection: null, Price: 0);

        while (unvisitedLocations.Count > 0)
        {
            var location = unvisitedLocations.OrderBy(location => cheapestConnections[location].Price).First();

            if (location == end)
            {
                break;
            }

            foreach (var connection in location.Connections)
            {
                UpdateCheapestConnections(cheapestConnections, location, connection);
            }

            unvisitedLocations.Remove(location);
        }

        return cheapestConnections;
    }

    private static void UpdateCheapestConnections(Dictionary<Location, (Connection? SourceConnection, decimal Price)> cheapestConnections, Location location, Connection connection)
    {
        var price = cheapestConnections[location].Price + connection.Price;

        if (price < cheapestConnections[connection.Destination].Price)
        {
            cheapestConnections[connection.Destination] = (SourceConnection: connection, Price: price);
        }
    }

    /// <summary>
    /// The shortest path is constructed by backtracking the Dijkstra connection data from the end location
    /// </summary>
    private static IEnumerable<Connection> ConstructCheapestPath(Location start, Location end, Dictionary<Location, (Connection? SourceConnection, decimal Price)> sourceConnections)
    {
        var path = new List<Connection>();
        var location = end;

        while (location != start)
        {
            var cheapestConnection = sourceConnections[location].SourceConnection!;
            path.Add(cheapestConnection);
            location = cheapestConnection.Source;
        }

        path.Reverse();

        return path;
    }
}
