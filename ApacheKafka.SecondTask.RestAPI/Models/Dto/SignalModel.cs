using System.ComponentModel.DataAnnotations;

namespace ApacheKafka.SecondTask.RestAPI.Models.Dto;

public class SignalModel
{
    [Required]
    public string VehicleId { get; set; } = string.Empty;

    [Required]
    public CoordinateModel StartCoordinate { get; set; } = new();

    [Required]
    public CoordinateModel EndCoordinate { get; set; } = new();
}
