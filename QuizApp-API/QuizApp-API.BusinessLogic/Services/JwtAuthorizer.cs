using Microsoft.AspNetCore.Identity;
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
    public class JwtAuthorizer(IConfiguration configuration, 
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager) 
        : IAuthorizer
    {
        private readonly IConfiguration _config = configuration;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;

        public async Task<string> Authorize(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    var token = GenerateToken(user);
                    return token;
                }
            }
            throw new Exception("Authorizer: Wrong user credentials");
        }

        private string GenerateToken(IdentityUser user)
        {
            var key = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
                throw new Exception("Jwt:key is null or empty");
            if(string.IsNullOrEmpty(user.UserName))
                throw new Exception("user.UserName is null or empty");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] {
                new Claim("UserId", user.Id),
                new Claim("Name", user.UserName)
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
