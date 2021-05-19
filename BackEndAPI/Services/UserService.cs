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

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

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
                throw new Exception("Unauthorized access");
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

        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll().WithoutPasswords();
        }

        //Generate JwtToken
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
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
        public async Task Disable(int id)
        {
            int userValid = _assignmentRepository.GetCountUser(id);
            var user = await _repository.GetById(id);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (userValid > 0)
            {
                throw new ArgumentException("User is still valid assignment");
            }
            else
            {
                user.Status = UserStatus.Disabled;
                await _repository.Update(user);
            }

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

                throw new Exception("User is under 18. Please select different date");
            }

            if (model.JoinedDate.DayOfWeek == DayOfWeek.Saturday
                   || model.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new Exception("Join Date is Saturday or Sunday. Please select different date");
            }

            if (model.JoinedDate < model.DateOfBirth)
            {

                throw new Exception("Join Date is not later than Date Of Birth. Please select different date");
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
    }
}