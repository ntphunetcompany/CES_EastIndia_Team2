using RoutePlanning.Domain.Locations;
using RoutePlanning.Domain.Locations.Services;

namespace RoutePlanning.Domain.UnitTests.Routing;

public sealed class PathTest
{
    [Fact]
    public void ShortestPathTest()
    {
        // Arrange
        var locationA = new Locations.Location("A");
        var locationB = new Locations.Location("B");
        var locationC = new Locations.Location("C");

        locationA.AddConnection(locationB, 2);
        locationB.AddConnection(locationC, 3);
        locationA.AddConnection(locationC, 6);

        var locations = new List<Locations.Location> { locationA, locationB, locationC };

        var shortestDistanceService = new ShortestDistanceService(locations.AsQueryable());

        // Act
        var distance = shortestDistanceService.CalculateShortestDistance(locationA, locationC);

        // Assert
        Assert.Equal(5, distance);
    }
}
