namespace ApacheKafka.Common.Models.Dto;

public class SignalInfo
{
    public string VehicleId { get; set; } = string.Empty;

    public CoordinateInfo StartCoordinate { get; set; } = new();

    public CoordinateInfo EndCoordinate { get; set; } = new();
}
