using System.Diagnostics;
using ApacheKafka.Common.Helpers;
using ApacheKafka.Common.Factories;
using ApacheKafka.Common.Interfaces;
using ApacheKafka.Common.Models.Dto;
using ApacheKafka.SecondTask.RestAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using static ApacheKafka.Common.KafkaConfiguration;

namespace ApacheKafka.SecondTask.RestAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IValidator<SignalInfo> _signalValidator;

    public VehicleController(IMapper mapper, IValidator<SignalInfo> signalValidator)
    {
        _mapper = mapper;
        _signalValidator = signalValidator;
    }

    [HttpPost]
    [Route("signal")]
    public async Task<IActionResult> Post(SignalModel signalModel)
    {
        var signalInfo = _mapper.Map<SignalInfo>(signalModel);

        using (var producer = KafkaFactory.CreateAtLeastOnceProducer<SignalInfo>(ServerUrl, Topics[1].Name, message => Debug.WriteLine(message)))
        {
            await KafkaHelper.CreateTopicsAsync(ServerUrl, Topics, message => Debug.WriteLine(message));

            var validationResult = _signalValidator.Validate(signalInfo);

            if (validationResult.IsError)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            await producer.SendMessageAsync(signalInfo);

            return Ok();
        }
    }
}
