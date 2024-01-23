using ApacheKafka.Common.Interfaces;
using ApacheKafka.Common.Models.Dto;
using System.Text.RegularExpressions;

namespace ApacheKafka.Common.Validators;

internal class VehicleIdValidator : IValidator<SignalInfo>
{
    public ResultInfo Validate(SignalInfo signalInfo)
    {
        if (!Regex.IsMatch(signalInfo.VehicleId, "^[A-Za-z]{3}[0-9]{3}$"))
        {
            return ResultInfo.CreateFailedResult($"VehicleId:{signalInfo.VehicleId} is invalid");
        }

        return ResultInfo.CreateSuccessfulResult();
    }
}
