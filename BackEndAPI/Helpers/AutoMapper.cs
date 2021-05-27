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
            CreateMap<AssignmentModel, Assignment>();
            CreateMap<User, UserDTO>();
            CreateMap<Assignment, AssignmentDTO>();
        }
    }
}