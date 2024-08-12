using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IFullUserProfileService
    {
        Task<UserProfileModel> GetFullUserProfileAsync(string username);
    }
}
