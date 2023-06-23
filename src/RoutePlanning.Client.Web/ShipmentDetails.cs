using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ShipmentDetails
{
    [Required]
    public string ShipmentType { get; set; }

    [Required]
    public decimal Length { get; set; }

    [Required]
    public decimal Width { get; set; }

    [Required]
    public decimal Height { get; set; }

    [Required]
    public decimal Weight { get; set; }

    [Required]
    public string Origin { get; set; }

    [Required]
    public string Destination { get; set; }

    [Required]
    public bool IsRecorded { get; set; }

    [Required]
    public DateTime DateOfShipment { get; set; }

    public ShipmentDetails()
    {
        Origin = "";
        ShipmentType = "";
        Destination = "";
    }

    [JsonExtensionData]
    public IDictionary<string, object>? ExtraFields { get; set; }
}
