using Netcompany.Net.DomainDrivenDesign.Services;

namespace RoutePlanning.Domain.Locations.Services;

public interface IShortestDistanceService : IDomainService
{
    IEnumerable<Connection> GetShortestPath(Location source, Location target);
    int CalculateShortestDistance(Location source, Location target);
}
