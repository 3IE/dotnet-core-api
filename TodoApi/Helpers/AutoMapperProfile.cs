using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Models;

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