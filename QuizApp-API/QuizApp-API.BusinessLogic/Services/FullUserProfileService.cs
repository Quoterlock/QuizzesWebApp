using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
namespace QuizApp_API.BusinessLogic.Services
{
    public class FullUserProfileService(
        IUserProfilesService userProfiles,
        IQuizzesService quizzesService) : IFullUserProfileService
    {
        private readonly IUserProfilesService _userProfiles = userProfiles;
        private readonly IQuizzesService _quizzesService = quizzesService;

        public async Task<UserProfileModel> GetFullUserProfile(string username)
        {
            var profileInfo = await _userProfiles.GetByUsernameAsync(username);
            var profile = new UserProfileModel
            {
                DisplayName = profileInfo.DisplayName,
                Id = profileInfo.Id,
                Owner = profileInfo.Owner,
                ImageId = profileInfo.ImageId,
                CompletedQuizzesCount = await _quizzesService.GetAllUserCompleted(profileInfo.Owner.Id),
                CreatedQuizzes = (await _quizzesService.GetAllTitlesByUserId(profileInfo.Owner.Id)).ToList()
            };
            return profile;
        }
    }
}
