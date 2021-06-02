using System.Linq;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BackEndAPI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using AutoMapper;

namespace BackEndAPI.Services
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly IAsyncUserRepository _repository;
        private readonly IAsyncAssignmentRepository _assignmentRepository;
        private readonly AppSettings _appSettings;
        private const int minimumAdmin = 2;

        public UserService(IAsyncUserRepository repository, IAsyncAssignmentRepository assignmentRepository, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _repository.GetAll().SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user == null) return null;
            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task<GetUsersListPagedResponseDTO> GetUsers(
            PaginationParameters paginationParameters,
            int adminId
        )
        {
            var adminUser = await _repository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception(Message.UnauthorizedUser);
            }

            var users = PagedList<User>.ToPagedList(
                _repository.GetAll()
                    .Where(u =>
                    u.Status == UserStatus.Active
                    && u.Location == adminUser.Location
                ),
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
                Items = users.Select(u => _mapper.Map<UserDTO>(u))
            };
        }

        #region Method to be removed
        public async Task<GetUsersListPagedResponseDTO> GetUsersByType(
            PaginationParameters paginationParameters,
            int adminId,
            UserType type
        )
        {
            var adminUser = await _repository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var users = PagedList<User>.ToPagedList(
                _repository.GetAll()
                    .Where(u =>
                    u.Status == UserStatus.Active
                    && u.Location == adminUser.Location
                    && u.Type == type
                ),
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
                Items = users.Select(u => _mapper.Map<UserDTO>(u))
            };
        }
        #endregion

        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll().WithoutPasswords();
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task Disable(int userId, int id)
        {
            int countAdmin = _repository.CountAdminRemain();
            int userValid = _assignmentRepository.GetCountUser(id);
            var user = await _repository.GetById(id);

            if (countAdmin < minimumAdmin && user.Type == UserType.Admin)
            {
                throw new Exception("System has only one admin remain");
            }

            if (userId == id)
            {
                throw new Exception("Can not disable yourself");
            }

            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (userValid > 0)
            {
                throw new ArgumentException("User is still valid assignment");
            }

            user.Status = UserStatus.Disabled;
            await _repository.Update(user);
        }

        public async Task Update(int id, EditUserModel model)
        {
            var user = await _repository.GetById(id);

            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (DateTime.Now.AddYears(-18) < model.DateOfBirth)
            {

                throw new Exception(Message.RestrictedAge);
            }

            if (model.JoinedDate.DayOfWeek == DayOfWeek.Saturday
                   || model.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new Exception(Message.WeekendJoinedDate);
            }

            if (model.JoinedDate < model.DateOfBirth)
            {

                throw new Exception(Message.JoinedBeforeBirth);
            }

            user.DateOfBirth = model.DateOfBirth;
            user.JoinedDate = model.JoinedDate;
            user.Gender = model.Gender;
            user.Type = model.Type;
            await _repository.Update(user);
        }

        public async Task<User> Create(CreateUserModel model)
        {

            if (model == null)
            {
                throw new ArgumentNullException(Message.NullUser);
            }

            bool isOlderThan18 = (model.DateOfBirth.Date <= DateTime.Now.Date.AddYears(-18));
            bool isEarlierThanDob = (model.JoinedDate.Date > model.DateOfBirth.Date);
            bool isWeekend = (model.JoinedDate.Date.DayOfWeek == DayOfWeek.Saturday || model.JoinedDate.Date.DayOfWeek == DayOfWeek.Sunday);

            if (!isOlderThan18)
            {

                throw new Exception(Message.RestrictedAge);

            }

            if (!isEarlierThanDob)
            {

                throw new Exception(Message.JoinedBeforeBirth);

            }


            if (isWeekend)
            {

                throw new Exception(Message.WeekendJoinedDate);

            }

            User user = _mapper.Map<User>(model);
            user.UserName = AutoGenerator.AutoGeneratedUsername(model.FirstName, model.LastName, _repository);

            User _user = await _repository.Create(user);

            _user.StaffCode = AutoGenerator.AutoGeneratedStaffCode(_user.Id);
            _user.Password = AutoGenerator.AutoGeneratedPassword(_user.UserName, model.DateOfBirth);
            await _repository.Update(_user);
            return _user;

        }

        public async Task<UserInfo> GetById(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            UserInfo userInfo = new UserInfo
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                JoinedDate = user.JoinedDate,
                Gender = user.Gender,
                Type = user.Type
            };
            return userInfo;
        }

        #region Method to be removed
        public async Task<GetUsersListPagedResponseDTO> SearchUsers(
            PaginationParameters paginationParameters,
            int adminId,
            string searchText
        )
        {
            if (searchText == null)
            {
                throw new Exception(Message.NullSearchQuery);
            }

            var adminUser = await _repository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception(Message.UnauthorizedUser);
            }

            var users = PagedList<User>.ToPagedList(
                _repository.GetAll()
                    .Where(u =>
                    u.Status == UserStatus.Active
                    && u.Location == adminUser.Location
                    &&
                    (
                        (u.FirstName + " " + u.LastName).StartsWith(searchText)
                        || u.StaffCode.StartsWith(searchText)
                    )
                    ),
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
                Items = users.Select(u => _mapper.Map<UserDTO>(u))
            };
        }
        #endregion

        public async Task ChangePassword(int id, ChangePasswordRequest model)
        {
            var user = await _repository.GetById(id);

            if (user.OnFirstLogin == OnFirstLogin.Yes)
            {
                user.OnFirstLogin = OnFirstLogin.No;
            }

            user.Password = model.NewPassword;

            await _repository.Update(user);
        }
        public async Task<User> GetUserByIdWithPassword(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null)
            {
                throw new InvalidOperationException(Message.UserNotFound);
            }

            return user;
        }

        public async Task<IQueryable<UserDTO>> GetAllUsers(int userId)
        {
            var user = await _repository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            var listUser = _repository.GetAll()
                               .Where(x => x.Location == user.Location && x.Status == UserStatus.Active)
                               .AsQueryable();
            var sendListUser = listUser.Select(x => _mapper.Map<UserDTO>(x));
            return sendListUser;
        }

        public async Task<IQueryable<UserDTO>> GetUserBySearching(int userId, string searchText)
        {
            var user = await _repository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            var listUser = _repository.GetAll()
                            .Where(x => x.Location == user.Location
                            && x.Status == UserStatus.Active
                            && (
                                 (x.FirstName + " " + x.LastName).Contains(searchText)
                                 || x.StaffCode.Contains(searchText)
                                )
                            )
                            .AsQueryable();
            var sendListUser = listUser.Select(x => _mapper.Map<UserDTO>(x));
            return sendListUser;
        }

        public async Task<GetUsersListPagedResponseDTO> SearchAndFilter(
            int adminId,
            UserSearchFilterParameters searchFilterParameters,
            PaginationParameters paginationParameters
        )
        {
            var adminUser = await _repository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var searchFilterRawResults = _repository.GetAll()
                .Where(u => u.Location == adminUser.Location
                        && u.Status == UserStatus.Active);
            if (searchFilterParameters.Type is not null)
            {
                searchFilterRawResults = searchFilterRawResults
                    .Where(u => u.Type == searchFilterParameters.Type);
            }

            searchFilterRawResults = searchFilterRawResults
                .Where(x => (x.FirstName + " " + x.LastName).Contains(searchFilterParameters.SearchQuery)
                            || x.StaffCode.Contains(searchFilterParameters.SearchQuery));

            var pagedListResult = PagedList<User>.ToPagedList(
                searchFilterRawResults,
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetUsersListPagedResponseDTO
            {
                CurrentPage = pagedListResult.CurrentPage,
                PageSize = pagedListResult.PageSize,
                TotalCount = pagedListResult.TotalCount,
                TotalPages = pagedListResult.TotalPages,
                HasNext = pagedListResult.HasNext,
                HasPrevious = pagedListResult.HasPrevious,
                Items = pagedListResult.Select(u => _mapper.Map<UserDTO>(u))
            };
        }
    }
}