using Microsoft.AspNetCore.Identity;
using Moq;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace UnitTests
{
    public class ProfilesServiceTests
    {
        [Fact]
        public async Task Get_user_profile_by_username_test()
        {
            // Arrange
            var profileId = "profileId";
            var ownerUserId = "ownerId";
            var profilesRepo = new Mock<IUserProfileRepository>();
            profilesRepo.Setup(m => m.GetByOwnerIdAsync(ownerUserId))
                .ReturnsAsync(new UserProfile { 
                    DisplayName = "Name",
                    Id = profileId,
                    ImageId = [],
                    OwnerId = ownerUserId,
                });

            var userService = new Mock<IUserService>();
            userService.Setup(m => m.GetByIdAsync(ownerUserId))
                .ReturnsAsync(new IdentityUser
                {
                    Id = ownerUserId,
                    UserName = "username"
                });

            var quizzesService = new Mock<IQuizzesService>();

            var quizzes = new List<QuizListItemModel>
                {
                    new QuizListItemModel() {
                        Author = { Id = profileId, DisplayName = "Name", Image = [], Owner = new ProfileOwnerInfo() { Id = ownerUserId, Username = "username" }},
                        Id = "1",
                        Rate = 100,
                        Title = "Title",
                    },
                }.AsEnumerable();

            quizzesService.Setup(m => m.GetAllTitlesByUserId(ownerUserId))
                .ReturnsAsync(quizzes);
            quizzesService.Setup(m => m.GetAllUserCompleted(ownerUserId))
                .ReturnsAsync(100);

            var sut = new UserProfilesService(profilesRepo.Object, userService.Object);

            var expected = new UserProfileModel
            {
                Id = profileId,
                DisplayName = "Name",
                CreatedQuizzes = quizzes.ToList(),
                CompletedQuizzesCount = 100,
                Owner = new ProfileOwnerInfo { Id = ownerUserId, Username = "username" }
            };

            // Act
            var actual = await sut.GetByOwnerId(ownerUserId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_user_profile_by_owner_id_test()
        {
            // Arrange
            var profileId = "profileId";
            var ownerUserId = "ownerId";
            var username = "username";
            var profilesRepo = new Mock<IUserProfileRepository>();
            profilesRepo.Setup(m => m.GetByOwnerIdAsync(ownerUserId))
                .ReturnsAsync(new UserProfile
                {
                    DisplayName = "Name",
                    Id = profileId,
                    ImageId = [],
                    OwnerId = ownerUserId,
                });

            var userService = new Mock<IUserService>();
            userService.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser
                {
                    Id = ownerUserId,
                    UserName = username
                });

            var quizzesService = new Mock<IQuizzesService>();

            var quizzes = new List<QuizListItemModel>
                {
                    new QuizListItemModel() {
                        Author = { Id = profileId, DisplayName = "Name", Image = [], Owner = new ProfileOwnerInfo() { Id = ownerUserId, Username = "username" }},
                        Id = "1",
                        Rate = 100,
                        Title = "Title",
                    },
                }.AsEnumerable();

            quizzesService.Setup(m => m.GetAllTitlesByUserId(ownerUserId))
                .ReturnsAsync(quizzes);
            quizzesService.Setup(m => m.GetAllUserCompleted(ownerUserId))
                .ReturnsAsync(100);

            var sut = new UserProfilesService(profilesRepo.Object, userService.Object);

            var expected = new UserProfileModel
            {
                Id = profileId,
                DisplayName = "Name",
                CreatedQuizzes = quizzes.ToList(),
                CompletedQuizzesCount = 100,
                Owner = new ProfileOwnerInfo { Id = ownerUserId, Username = "username" }
            };

            // Act
            var actual = await sut.GetByUsernameAsync(ownerUserId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_user_profiles_info_for_multiple_users_test()
        {
            Assert.True(false);
        }
    }
}
