using Moq;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace UnitTests
{
    public class RatesServiceTests
    {
        [Fact]
        public async void Get_rates_for_multiple_quizzes()
        {
            // Arrange
            string[] ids = ["1", "2", "3"];
            var mockRepo = new Mock<IRatesRepository>();
            mockRepo.Setup(m => m.Get(ids))
                .ReturnsAsync(new List<UserQuizRate>
                {
                    new UserQuizRate { Id = "0", QuizId = "1", Rate = 10, UserId = "0"},
                    new UserQuizRate { Id = "1", QuizId = "1", Rate = 11, UserId = "0"},
                    new UserQuizRate { Id = "2", QuizId = "2", Rate = 12, UserId = "0"},
                    new UserQuizRate { Id = "3", QuizId = "3", Rate = 13, UserId = "0"},
                    new UserQuizRate { Id = "4", QuizId = "3", Rate = 14, UserId = "0"},
                });
            var expected = new List<QuizRateModel>
            {
                    new QuizRateModel { Id = "0", QuizId = "1", Rate = 10, UserId = "0"},
                    new QuizRateModel { Id = "1", QuizId = "1", Rate = 11, UserId = "0"},
                    new QuizRateModel { Id = "2", QuizId = "2", Rate = 12, UserId = "0"},
                    new QuizRateModel { Id = "3", QuizId = "3", Rate = 13, UserId = "0"},
                    new QuizRateModel { Id = "4", QuizId = "3", Rate = 14, UserId = "0"},
            };

            var sut = new RatesService(mockRepo.Object);

            // Act
            var result = await sut.GetRatesAsync(ids);

            // Assert
            Assert.Equal(expected, result);
        } 
    }
}
