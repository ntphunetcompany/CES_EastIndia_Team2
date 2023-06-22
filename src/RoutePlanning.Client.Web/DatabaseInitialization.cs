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
        var karstaden = new Location("Karstaden");
        await context.AddAsync(karstaden);



        var hvalbugten = new Location("Hvalbugten");
        await context.AddAsync(hvalbugten);



        var luanda = new Location("Luanda");
        await context.AddAsync(luanda);



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

        var pap_Guardafui = new Location("PapGuardafui");
        await context.AddAsync(pap_Guardafui);

        var mocambique = new Location("Mocambique");
        await context.AddAsync(mocambique);

        var amatave = new Location("Amatave");
        await context.AddAsync(amatave);



        var kap_ST_Marie = new Location("Kat_ST_Marie");
        var locations = new List<Location> { karstaden, hvalbugten, luanda, sT_Helena, slavekysten, guldKysten, sierra_Leone, dakar, dekanariske_Øer, tanger, tunis, cairo, suakin, pap_Guardafui, mocambique, amatave, kap_ST_Marie };
        SeedAllConnections(locations);
        await context.AddAsync(kap_ST_Marie);
        
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
        var alice = new User("alice", User.ComputePasswordHash("alice123!"));
        await context.AddAsync(alice);

        var bob = new User("bob", User.ComputePasswordHash("!CapableStudentCries25"));
        await context.AddAsync(bob);
    }

    private static void CreateTwoWayConnection(Location locationA, Location locationB, int distance, int timeInHours,
        int price)
    {
        locationA.AddConnection(locationB, distance);
        locationB.AddConnection(locationA, distance);
    }
}
