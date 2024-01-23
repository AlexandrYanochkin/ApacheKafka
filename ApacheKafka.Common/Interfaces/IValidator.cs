using ApacheKafka.Common.Models.Dto;

namespace ApacheKafka.Common.Interfaces;

public interface IValidator<TInput>
{
   ResultInfo Validate(TInput input);
}
