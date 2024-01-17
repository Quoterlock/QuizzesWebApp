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
    public class JwtAuthorizer : IAuthorizer
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public JwtAuthorizer(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
        { 
            _config = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
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
