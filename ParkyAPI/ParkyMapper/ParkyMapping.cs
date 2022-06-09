using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOS;

namespace ParkyAPI.ParkyMapper
{
    public class ParkyMapping : Profile
    {
        public ParkyMapping()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
        }
    }
}
