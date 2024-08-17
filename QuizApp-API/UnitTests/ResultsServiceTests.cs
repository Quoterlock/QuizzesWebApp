using Moq;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace UnitTests
{
    public class ResultsServiceTests
    {
        [Fact]
        public async Task Get_results_by_quiz_id_test()
        {
            var quizResultsRepo = new Mock<IQuizResultsRepository>();
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizResultsService(
                quizResultsRepo.Object, 
                profilesService.Object);

            var quizId = "1";
            var results = new List<QuizResult>() 
            {
                new QuizResult() { Id = "1", QuizId = quizId, Result = 10, TimeStamp = "", UserId = "0"},
                new QuizResult() { Id = "2", QuizId = quizId, Result = 11, TimeStamp = "", UserId = "3"},
            };
            quizResultsRepo.Setup(m => m.GetByQuizIdAsync(quizId))
                .ReturnsAsync(results);
            var profiles = new List<UserProfileInfo>()
            {
                new UserProfileInfo {
                        DisplayName = "Name", Id = "1", ImageId = "0",
                        Owner = new ProfileOwnerInfo {
                            Id = "0", Username = ""
                        }
                    },
                new UserProfileInfo {
                        DisplayName = "Name", Id = "1", ImageId = "0",
                        Owner = new ProfileOwnerInfo {
                            Id = "3", Username = ""
                        }
                    },
            };
            string[] userIds = ["0", "3"];
            profilesService.Setup(m => m.GetRangeAsync(userIds))
                .ReturnsAsync(profiles);

            var expected = new List<QuizResultModel>()
            {
                new QuizResultModel() { Id = "1", QuizId = quizId, Result = 10, TimeStamp="",
                    UserProfile = profiles[0]
                },
                new QuizResultModel() { Id = "2", QuizId = quizId, Result = 11, TimeStamp="",
                    UserProfile = profiles[1]
                }
            };

            // Act
            var actual = await sut.GetResultsByQuizIdAsync(quizId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_results_by_user_id_test()
        {
            var quizResultsRepo = new Mock<IQuizResultsRepository>();
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizResultsService(
                quizResultsRepo.Object,
                profilesService.Object);

            var userId = "userId";
            var results = new List<QuizResult>()
            {
                new QuizResult() { Id = "1", QuizId = "q1", Result = 10, TimeStamp = "", UserId = userId},
                new QuizResult() { Id = "2", QuizId = "q2", Result = 11, TimeStamp = "", UserId = userId},
            };
            quizResultsRepo.Setup(m => m.GetByUserIdAsync(userId))
                .ReturnsAsync(results);
            var profiles = new List<UserProfileInfo>()
            {
                new UserProfileInfo {
                        DisplayName = "Name", Id = "1", ImageId = "0",
                        Owner = new ProfileOwnerInfo {
                            Id = userId, Username = ""
                        }
                    },
            };
            string[] userIds = [userId, userId];
            profilesService.Setup(m => m.GetRangeAsync(userIds))
                .ReturnsAsync(profiles);

            var expected = new List<QuizResultModel>()
            {
                new QuizResultModel() { Id = "1", QuizId = "q1", Result = 10, TimeStamp="",
                    UserProfile = profiles[0]
                },
                new QuizResultModel() { Id = "2", QuizId = "q2", Result = 11, TimeStamp="",
                    UserProfile = profiles[0]
                }
            };

            // Act
            var actual = await sut.GetResultsByUserIdAsync(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Save_quiz_result_test()
        {
            // Arrange
            var quizResultsRepo = new Mock<IQuizResultsRepository>();
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizResultsService(
                quizResultsRepo.Object,
                profilesService.Object);

            var userId = "userid";
            var quizId = "quizid";
            var result = 10;

            var quizResult = new QuizResult()
            {
                QuizId = quizId, Id = "", Result = result, 
                TimeStamp = "", UserId = userId
            };

            // Act
            await sut.SaveResultAsync(quizId, userId, result);

            // Assert
            quizResultsRepo.Verify(
                m => m.SaveResultAsync(It.Is<QuizResult>(r => r.Equals(quizResult))),
                Times.Once);
        }

        [Fact]
        public async Task Save_quiz_result_with_no_userid_test()
        {
            // Arrange
            var quizResultsRepo = new Mock<IQuizResultsRepository>();
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizResultsService(
                quizResultsRepo.Object,
                profilesService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => sut.SaveResultAsync("quiz", "", 10));
        }

        [Fact]
        public async Task Save_quiz_result_with_no_quizId_test()
        {
            // Arrange
            var quizResultsRepo = new Mock<IQuizResultsRepository>();
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizResultsService(
                quizResultsRepo.Object,
                profilesService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => sut.SaveResultAsync("", "user", 10));
        }
    }
}
