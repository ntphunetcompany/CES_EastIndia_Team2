using Netcompany.Net.DomainDrivenDesign.Services;

namespace RoutePlanning.Domain.Locations.Services;

public interface IShortestDistanceService : IDomainService
{
    IEnumerable<Connection> GetShortestPath(Location source, Location target);
    int CalculateShortestDistance(Location source, Location target);
}

public interface ICheapestDistanceService : IDomainService
{
    IEnumerable<Connection> GetCheapestPath(Location source, Location target);
    decimal CalculateCheapestDistance(Location source, Location target);
}
