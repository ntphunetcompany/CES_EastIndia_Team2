using System.Text;
using Netcompany.Net.Testing.Api;
using RoutePlanning.Client.Web.Api;
using Xunit.Abstractions;

namespace RoutePlanning.Client.Web.ApiTests.Authentication;

public class TokenTests : IClassFixture<RoutePlanningApplicationFactory>
{
    private readonly RoutePlanningApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public TokenTests(RoutePlanningApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = _factory.HttpClient;
        _output = output;
    }

    [Fact]
    public async void ShouldGetHelloWorld()
    {
        // Arrange
        var url = _factory.GetRoute<Program, RoutesController>(x => x.HelloWorld);

        //api/routes/HelloWorld
        _output.WriteLine("url: ");
        _output.WriteLine(url);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Token", "TheSecretApiToken");

        var response = await _client.SendAsync(request);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World!", content);
    }

    [Fact]
    public async void ShouldGetShortestRoute()
    {
        // Arrange
        var url = _factory.GetRoute<Program, RoutesController>(x => x.RequestPrice);

        //api/routes/HelloWorld
        _output.WriteLine("url: ");
        _output.WriteLine(url);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Token", "TheSecretApiToken");
        
        request.Content = new StringContent("{\"ShipmentType\":\"weapons\",\"Length\":33,\"Width\":32,\"Height\":33,\"Weight\":43,\"Origin\":\"kapstaden\",\"Destination\":\"kap_Guardafui\",\"IsRecorded\":false,\"DateOfShipment\":\"2022-01-01\"}",
                                    Encoding.UTF8,
                                    "application/json");
        //request.Content.Headers.Add("Content-Type", "Application/Json");

        var response = await _client.SendAsync(request);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        _output.WriteLine(content);
        // Assert.Equal("Hello World!", content);
    }

    [Fact]
    public async void ShouldBook()
    {
        // Arrange
        var url = _factory.GetRoute<Program, RoutesController>(x => x.Book);

        //api/routes/HelloWorld
        _output.WriteLine("url: ");
        _output.WriteLine(url);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Token", "TheSecretApiToken");

        request.Content = new StringContent("{\"ShipmentType\":\"John Doe\",\"Length\":33,\"Width\":32,\"Height\":33,\"Weight\":43,\"Origin\":\"kapstaden\",\"Destination\":\"St_Helena\",\"IsRecorded\":false,\"DateOfShipment\":\"2022-01-01\"}",
                                    Encoding.UTF8,
                                    "application/json");
        //request.Content.Headers.Add("Content-Type", "Application/Json");

        var response = await _client.SendAsync(request);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        _output.WriteLine(content);
        // Assert.Equal("Hello World!", content);
    }
}
