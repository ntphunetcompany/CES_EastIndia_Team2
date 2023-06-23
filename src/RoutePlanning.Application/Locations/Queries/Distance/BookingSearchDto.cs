namespace RoutePlanning.Application.Locations.Queries.Distance;

public record struct BookingSearchDto(
    int Distance,
    int Price,
    int HoursToShip
);
