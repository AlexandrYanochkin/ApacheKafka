using System.ComponentModel.DataAnnotations;

namespace ApacheKafka.SecondTask.RestAPI.Models.Dto;

public class CoordinateModel
{
    [Required]
    public double XValue { get; set; }

    [Required]
    public double YValue { get; set; }
}
