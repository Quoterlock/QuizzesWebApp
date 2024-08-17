using Microsoft.AspNetCore.Identity;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task Register(string username, string email, string password);
        Task<string> Authorize(string email, string password);
        Task RemoveUser(string username, string email);
        Task<IdentityUser> GetByIdAsync(string id);
        Task<IdentityUser> GetByNameAsync(string username);
        Task<IEnumerable<IdentityUser>> GetRangeByIdAsync(params string[] userIds);
    }
}
