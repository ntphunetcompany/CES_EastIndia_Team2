using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RouteDetails
{
    [Required]
    public string Origin { get; set; }

    [Required]
    public string Destination { get; set; }

    [Required]
    public int Duration { get; set; }

    [Required]
    public int Cost { get; set; }


    public RouteDetails()
    {
        Origin = "";
        Destination = "";
        Cost = 0;
        Duration = 0;
    }

    [JsonExtensionData]
    public IDictionary<string, object>? ExtraFields { get; set; }
}
