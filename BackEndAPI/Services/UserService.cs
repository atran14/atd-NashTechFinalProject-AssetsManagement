using System.Linq;
using System.Collections.Generic;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using System;
using BackEndAPI.Helpers;

namespace BackEndAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncUserRepository _repository;

        public UserService(IAsyncUserRepository repository)
        {
            _repository = repository;
        }

        public GetUsersListPagedResponseDTO GetUsers(PaginationParameters paginationParameters)
        {
            var users = PagedList<User>.ToPagedList(
                _repository.GetAll().Where(u => u.Status == UserStatus.Active),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetUsersListPagedResponseDTO
            {
                CurrentPage = users.CurrentPage,
                PageSize = users.PageSize,
                TotalCount = users.TotalCount,
                TotalPages = users.TotalPages,
                HasNext = users.HasNext,
                HasPrevious = users.HasPrevious,
                Items = users.Select(u => UserToDTO(u))
            };
        }

        private UserDTO UserToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                JoinedDate = user.JoinedDate,
                Location = user.Location,
                StaffCode = user.StaffCode,
                Type = user.Type,
            };
        }
    }
}