using BackEndAPI.Repositories;
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
using BackEndAPI.DBContext;
using System.Linq;

namespace BackEndAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncUserRepository _repository;
        private readonly AppSettings _appSettings;

        public UserService(IAsyncUserRepository repository, AssetsManagementDBContext context, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
        }
        
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _repository.GetAll().SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
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
  }
}