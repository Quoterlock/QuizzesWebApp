using Microsoft.AspNetCore.Identity;
using QuizApp_API.BusinessLogic.Interfaces;
using SharpCompress.Common;
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

        public async Task<IdentityUser> GetByIdAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) 
                throw new Exception($"User with id:{id} doesn't exsit");

            return user;
        }

        public async Task<IdentityUser> GetByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null)
                throw new Exception($"User with username:{username} doesn't exsit");
            return user;
        }

        public async Task<IEnumerable<IdentityUser>> GetRangeByIdAsync(params string[] userIds)
        {
            var users = _userManager.Users.Where(u => userIds.Contains(u.Id));
            return users;
        }

        public async Task Register(string username, string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true // just for testing
            };

            if (!await EmailIsUsed(user.Email))
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

        private async Task<bool> EmailIsUsed(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
