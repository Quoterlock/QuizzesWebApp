using Microsoft.AspNetCore.Http;
using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IUserProfilesService
    {
        Task<List<UserProfileInfo>> GetRangeAsync(params string[] userIds);
        Task<UserProfileInfo> GetByUsernameAsync(string username);
        Task<bool> IsExistsAsync(string username);
        Task CreateAsync(string username);
        Task UpdateAsync(UserProfileInfo profile);
        Task DeleteAsync(UserProfileInfo profile);
        Task UpdateProfilePhotoAsync(byte[] image, string username);
        Task<byte[]> GetProfilePhotoAsync(string username);
    }
}
