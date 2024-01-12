using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Services
{
    public class JwtAuthorizer : IAuthorizer
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _config;
        public JwtAuthorizer(IConfiguration configuration, IUserManager userManager) 
        { 
            _userManager = userManager;
            _config = configuration;
        }
        public string Authorize(string username, string password)
        {
            try
            {
                var user = _userManager.GetUser(u => u.Username == username && u.Password == password);
                var token = GenerateToken(user);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("Authorizer: " + ex.Message);
            }
        }

        private string GenerateToken(MockAppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] {
                new Claim("UserId", user.Id)
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"], 
                _config["Jwt:Audience"], 
                claims, 
                expires: DateTime.Now.AddDays(2), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
