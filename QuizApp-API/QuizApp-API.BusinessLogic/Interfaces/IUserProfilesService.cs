using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IUserProfilesService
    {
        Task<UserProfileModel> GetByIdAsync(string profileId);
        Task<UserProfileModel> GetByUsernameAsync(string username);
        Task<bool> IsExists(string username);
        Task CreateAsync(string username);
        Task UpdateAsync(UserProfileModel profile);
        Task DeleteAsync(UserProfileModel profile);

    }
}
