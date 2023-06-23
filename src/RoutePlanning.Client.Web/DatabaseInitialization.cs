using Netcompany.Net.UnitOfWork;
using RoutePlanning.Domain.Locations;
using RoutePlanning.Domain.Users;
using RoutePlanning.Infrastructure.Database;

namespace RoutePlanning.Client.Web;

public static class DatabaseInitialization
{
    public static async Task SeedDatabase(WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<RoutePlanningDatabaseContext>();
        await context.Database.EnsureCreatedAsync();

        var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        await using (var unitOfWork = unitOfWorkManager.Initiate())
        {
            await SeedUsers(context);
            await SeedLocationsAndRoutes(context);

            unitOfWork.Commit();
        }
    }

    private static async Task SeedLocationsAndRoutes(RoutePlanningDatabaseContext context)
    { 
        var kapstaden = new Location("Kapstaden");
        await context.AddAsync(kapstaden);



        var hvalbugten = new Location("Hvalbugten");
        await context.AddAsync(hvalbugten);


        var sT_Helena = new Location("ST_Helena");
        await context.AddAsync(sT_Helena);



        var slavekysten = new Location("Slavekysten");
        await context.AddAsync(slavekysten);



        var guldKysten = new Location("Guldkysten");
        await context.AddAsync(guldKysten);



        var sierra_Leone = new Location("Sierra_Leone");
        await context.AddAsync(sierra_Leone);

        var dakar = new Location("Dakar");
        await context.AddAsync(dakar);



        var dekanariske_Øer = new Location("Dekanariske_Øer");
        await context.AddAsync(dekanariske_Øer);



        var tanger = new Location("Tanger");
        await context.AddAsync(tanger);



        var tunis = new Location("Tunis");
        await context.AddAsync(tunis);



        var cairo = new Location("Cairo");
        await context.AddAsync(cairo);



        var suakin = new Location("Suakin");
        await context.AddAsync(suakin);




        var kap_Guardafui = new Location("Kap_Guardafui");
        await context.AddAsync(kap_Guardafui);



        var mocambique = new Location("Mocambique");
        await context.AddAsync(mocambique);



        var amatave = new Location("Amatave");
        await context.AddAsync(amatave);



        var kap_ST_Marie = new Location("Kap_ST_Marie");
        await context.AddAsync(kap_ST_Marie);

        CreateTwoWayConnection(kapstaden, hvalbugten, 3, 5, 1000);
        CreateTwoWayConnection(kapstaden, sT_Helena, 9, 6, 1300);
        CreateTwoWayConnection(kapstaden, kap_ST_Marie, 8, 8, 2300);
        CreateTwoWayConnection(hvalbugten, slavekysten, 9, 3, 3420);
        CreateTwoWayConnection(hvalbugten, guldKysten, 11, 2, 6230);
        CreateTwoWayConnection(hvalbugten, sT_Helena, 10, 8, 2310);
        CreateTwoWayConnection(sT_Helena, sierra_Leone, 11, 9, 2300);
        CreateTwoWayConnection(sT_Helena, dakar, 10, 10, 6230);
        CreateTwoWayConnection(sierra_Leone, guldKysten, 4, 5, 2310);
        CreateTwoWayConnection(dakar, dekanariske_Øer, 5, 2, 4120);
        CreateTwoWayConnection(dekanariske_Øer, tanger, 3, 7, 2300);
        CreateTwoWayConnection(tanger, tunis, 3, 3, 1220);
        CreateTwoWayConnection(tunis, cairo, 5, 1, 3110);
        CreateTwoWayConnection(cairo, suakin, 4, 6, 4120);
        CreateTwoWayConnection(kap_Guardafui, amatave, 8, 4, 3320);
        CreateTwoWayConnection(kap_Guardafui, mocambique, 8, 7, 2300);
        CreateTwoWayConnection(mocambique, kap_ST_Marie, 3, 8, 3200);
    }

    private static void SeedAllConnections(List<Location> locations)
    {
        var random = new Random();

        for (var i = 0; i < locations.Count; i++)
        {
            for (var j = i + 1; j < locations.Count; j++)
            {
                // Generate random distance, time and price for each connection
                var distance = random.Next(1, 20);
                var timeInHours = random.Next(1, 24);
                var price = random.Next(500, 5000);

                CreateTwoWayConnection(locations[i], locations[j], distance, timeInHours, price);
            }
        }
    }


    private static async Task SeedUsers(RoutePlanningDatabaseContext context)
    {
        var phunguyen = new User("phunguyen", User.ComputePasswordHash("phu123!"));
        await context.AddAsync(phunguyen);

        var orhan = new User("orhan", User.ComputePasswordHash("orhan123!"));
        await context.AddAsync(orhan);
    }

    private static void CreateTwoWayConnection(Location locationA, Location locationB, int distance, int timeInHours,
        int price)
    {
        locationA.AddConnection(locationB, distance);
        locationB.AddConnection(locationA, distance);
    }
}
