using ApacheKafka.Common.Models.Dto;

namespace ApacheKafka.Common.Helpers;

public static class VehicleHelper
{
    private const int Degree = 2;

    public static double CalculateDistance(this SignalInfo signalInfo)
    {
        var fCoordinate = signalInfo.StartCoordinate;
        var sCoordinate = signalInfo.EndCoordinate;
        var distance = Math.Sqrt(Math.Pow(fCoordinate.XValue - sCoordinate.XValue, Degree) + Math.Pow(fCoordinate.YValue - sCoordinate.YValue, Degree));

        return distance;
    }
}
