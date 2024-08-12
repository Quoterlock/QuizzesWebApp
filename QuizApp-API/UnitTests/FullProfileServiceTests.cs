using Moq;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;

namespace UnitTests
{
    public class FullProfileServiceTests
    {
        [Fact]
        public async Task Get_full_profile_test()
        {
            // Arrange
            var username = "username";

            var userProfilesService = new Mock<IUserProfilesService>();
            var quizzesService = new Mock<IQuizzesService>();

            var sut = new FullUserProfileService(
                userProfilesService.Object,
                quizzesService.Object);

            var profile = new UserProfileInfo
            {
                Id = "1", DisplayName = "User", ImageId = "0",
                Owner = new ProfileOwnerInfo { Id = "2", Username = username }
            };
            QuizListItemModel[] quizListItems = [
                    new QuizListItemModel { Id = "11", Author = profile, Rate = 10, Title = "" },
                    new QuizListItemModel { Id = "22", Author = profile, Rate = 9, Title = "" },
                    new QuizListItemModel { Id = "33", Author = profile, Rate = 4, Title = "" },

                ];

            userProfilesService.Setup(m => m.GetByUsernameAsync(username))
                .ReturnsAsync(profile);
            quizzesService.Setup(m => m.GetAllUserCompletedAsync("2"))
                .ReturnsAsync(100);
            quizzesService.Setup(m => m.GetAllTitlesByUserIdAsync("2"))
                .ReturnsAsync(quizListItems.ToList());

            var expected = new UserProfileModel
            {
                Id = profile.Id,
                DisplayName = profile.DisplayName,
                ImageId = profile.ImageId,
                Owner = profile.Owner,
                CompletedQuizzesCount = 100,
                CreatedQuizzes = quizListItems.ToList(),
            };

            // Act
            var actual = await sut.GetFullUserProfileAsync(username);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_full_profile_for_non_existring_user_test()
        {
            // Arrange
            var username = "username";

            var userProfilesService = new Mock<IUserProfilesService>();
            var quizzesService = new Mock<IQuizzesService>();

            var sut = new FullUserProfileService(
                userProfilesService.Object,
                quizzesService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(()=>sut.GetFullUserProfileAsync(username));
        }
    }
}
