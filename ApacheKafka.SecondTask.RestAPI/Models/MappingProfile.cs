using ApacheKafka.Common.Models.Dto;
using ApacheKafka.SecondTask.RestAPI.Models.Dto;
using AutoMapper;

namespace ApacheKafka.SecondTask.RestAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CoordinateModel, CoordinateInfo>().ReverseMap();
        CreateMap<SignalModel, SignalInfo>().ReverseMap();
    }
}
