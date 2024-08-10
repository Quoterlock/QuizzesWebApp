using Microsoft.AspNetCore.Http;
using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IUserProfilesService
    {
        Task<UserProfileInfo> GetByOwnerId(string ownerUserId);
        Task<List<UserProfileInfo>> GetRangeAsync(params string[] userIds);
        Task<UserProfileInfo> GetByUsernameAsync(string username);
        Task<bool> IsExists(string username);
        Task CreateAsync(string username);
        Task UpdateAsync(UserProfileInfo profile);
        Task DeleteAsync(UserProfileInfo profile);
        Task UpdateProfilePhoto(IFormFile img, string username);

    }
}
