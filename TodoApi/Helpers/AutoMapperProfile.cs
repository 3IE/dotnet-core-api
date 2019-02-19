using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Entities;

namespace TodoApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Users, UserDto>();
            CreateMap<UserDto, Users>();
        }
    }
}