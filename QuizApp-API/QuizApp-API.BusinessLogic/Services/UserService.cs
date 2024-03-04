using Microsoft.AspNetCore.Identity;
using QuizApp_API.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthorizer _authorizer;

        public UserService(UserManager<IdentityUser> userManager, IAuthorizer authorizer)
        { 
            _userManager = userManager;
            _authorizer = authorizer;
        }

        public async Task<string> Authorize(string email, string password)
        {
            return await _authorizer.Authorize(email, password);
        }

        public async Task Register(string username, string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true // just for testing
            };

            if (!await emailIsUsed(user.Email))
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    string errors = string.Empty;
                    foreach (var error in result.Errors)
                        errors += (error.Description + " ");
                    throw new Exception(errors);
                }
            }
            else throw new Exception("Email is already taken");
        }

        public Task RemoveUser(string username, string email)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> emailIsUsed(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
