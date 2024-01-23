namespace ApacheKafka.Common.Models.Dto;

public record ResultInfo(bool IsError, string ErrorMessage)
{
    public static ResultInfo CreateSuccessfulResult()
    {
        return new ResultInfo(false, string.Empty);
    }

    public static ResultInfo CreateFailedResult(string errorMessage)
    {
        return new ResultInfo(true, errorMessage);
    }
}

public record ResultInfo<TValue>(bool IsError, TValue? Value, string ErrorMessage)
{
    public static ResultInfo<TValue> CreateSuccessfulResult(TValue value)
    {
        return new ResultInfo<TValue>(false, value, string.Empty);
    }

    public static ResultInfo<TValue> CreateFailedResult(string errorMessage) 
    {
        return new ResultInfo<TValue>(true, default, errorMessage);
    }
}