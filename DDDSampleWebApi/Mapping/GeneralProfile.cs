using AutoMapper;
using DDDSampleWebApi.DTOs;
using DDDSampleWebApi.Models;

namespace DDDSampleWebApi.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<Hiker, HikerDto>().ReverseMap();
        CreateMap<Location, LocationDto>().ReverseMap();
        CreateMap<Item, ItemDto>().ReverseMap();
    }
}