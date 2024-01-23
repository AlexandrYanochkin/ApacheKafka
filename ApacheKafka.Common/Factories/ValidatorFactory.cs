using ApacheKafka.Common.Models.Dto;
using ApacheKafka.Common.Interfaces;
using ApacheKafka.Common.Validators;

namespace ApacheKafka.Common.Factories;

public static class ValidatorFactory
{
    public static IValidator<SignalInfo> CreateSignalValidator()
    {
        return new VehicleIdValidator();
    }
}
