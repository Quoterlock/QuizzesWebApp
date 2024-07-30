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
        public async Task Get_user_profile_info_by_id()
        {
            // Arrange
            string profileId = "11";
            string userId = "1";
            var userProfile = new UserProfile() { 
                 DisplayName = "name",
                 Id = profileId,
                 OwnerId = userId
            };
            var userIdentity = new IdentityUser { 
                UserName = "username", 
                Id = userId 
            };
            var quizzes = new List<QuizListItemModel>() {
                new QuizListItemModel {
                    Author = "username", AuthorId = profileId,
                    Id = "123", Rate = 10, Title = "none"
                },
                new QuizListItemModel {
                    Author = "username", AuthorId = profileId,
                    Id = "1234", Rate = 10, Title = "none"
                },
                new QuizListItemModel {
                    Author = "username1", AuthorId = "another",
                    Id = "12345", Rate = 10, Title = "none"
                },
            };
            var expected = new UserProfileModel()
            {
                DisplayName = "name",
                Id = profileId,
                Owner = new ProfileOwnerInfo()
                {
                    Username = "username",
                    Id = userId
                },
                CreatedQuizzes = quizzes
            }; 

            // mock services
            var userProfilesRepoMock = new Mock<IUserProfileRepository>();
            userProfilesRepoMock.Setup(m => m.GetByIdAsync(profileId))
                .ReturnsAsync(userProfile);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(m => m.GetByIdAsync(userId))
                .ReturnsAsync(userIdentity);

            var quizzesServiceMock = new Mock<IQuizzesService>();
            quizzesServiceMock.Setup(m => m.GetAllTitlesByUserId(profileId))
                .ReturnsAsync(quizzes);

            var sut = new UserProfilesService(
                userProfilesRepoMock.Object, 
                userServiceMock.Object,
                quizzesServiceMock.Object);

            // Act
            var actual = await sut.GetByIdAsync(profileId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_user_profile_info_by_username()
        {
            // Arrange
            string profileId = "11";
            string userId = "1";
            string username = "username";
            var userProfile = new UserProfile()
            {
                DisplayName = "name",
                Id = profileId,
                OwnerId = userId
            };
            var userIdentity = new IdentityUser
            {
                UserName = username,
                Id = userId
            };
            var quizzes = new List<QuizListItemModel>() {
                new QuizListItemModel {
                    Author = username, AuthorId = profileId,
                    Id = "123", Rate = 10, Title = "none"
                },
                new QuizListItemModel {
                    Author = username, AuthorId = profileId,
                    Id = "1234", Rate = 10, Title = "none"
                },
                new QuizListItemModel {
                    Author = "username1", AuthorId = "another",
                    Id = "12345", Rate = 10, Title = "none"
                },
            };
            var expected = new UserProfileModel()
            {
                DisplayName = "name",
                Id = profileId,
                Owner = new ProfileOwnerInfo()
                {
                    Username = username,
                    Id = userId
                },
                CreatedQuizzes = quizzes
            };

            // mock services
            var userProfilesRepoMock = new Mock<IUserProfileRepository>();
            userProfilesRepoMock.Setup(m => m.GetByOwnerIdAsync(userId))
                .ReturnsAsync(userProfile);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(userIdentity);

            var quizzesServiceMock = new Mock<IQuizzesService>();
            quizzesServiceMock.Setup(m => m.GetAllTitlesByUserId(profileId))
                .ReturnsAsync(quizzes);

            var sut = new UserProfilesService(
                userProfilesRepoMock.Object,
                userServiceMock.Object,
                quizzesServiceMock.Object);

            // Act
            var actual = await sut.GetByUsernameAsync(username);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
