using AutoMapper;
using BackEndAPI.Entities;
using BackEndAPI.Models;

namespace BackEndAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserModel, User>();
            CreateMap<User, UserDTO>();
        }
    }
}