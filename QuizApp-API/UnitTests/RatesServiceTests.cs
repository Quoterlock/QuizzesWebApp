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
        public async Task Get_rates_for_multiple_quizzes()
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

        [Fact]
        public async Task Add_rate_test()
        {
            // Arrange
            var ratesRepo = new Mock<IRatesRepository>();
            var sut = new RatesService(ratesRepo.Object);

            var userId = "user";
            var quizId = "quiz";
            var rate = 10;

            var quizRate = new UserQuizRate
            {
                Id = "", QuizId = quizId, 
                UserId = userId, Rate = rate
            };

            // Act
            await sut.AddRateAsync(quizId, userId, rate);

            // Assert
            ratesRepo.Verify(
                m => m.AddAsync(It.Is<UserQuizRate>(e => e.Equals(quizRate))),
                Times.Once);
        }

        [Fact]
        public async Task Add_rate_over_limit_test()
        {
            // Arrange
            var ratesRepo = new Mock<IRatesRepository>();
            var sut = new RatesService(ratesRepo.Object);

            var userId = "user";
            var quizId = "quiz";
            var rate = 101;

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.AddRateAsync(quizId, userId, rate));
        }

        [Fact]
        public async Task Add_rate_under_limit_test()
        {
            // Arrange
            var ratesRepo = new Mock<IRatesRepository>();
            var sut = new RatesService(ratesRepo.Object);

            var userId = "user";
            var quizId = "quiz";
            var rate = -1;

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => sut.AddRateAsync(quizId, userId, rate));
        }

        [Fact]
        public async Task Update_rate_test()
        {
            // Arrange
            var ratesRepo = new Mock<IRatesRepository>();
            var sut = new RatesService(ratesRepo.Object);

            var rateId = "rateid";
            double rateValue = 99;
            var rate = new UserQuizRate { 
                Id = rateId, QuizId = "1", 
                Rate = 10, UserId = "userid" 
            };
            ratesRepo.Setup(m => m.GetByIdAsync(rateId))
                .ReturnsAsync(rate);

            var expectedRate = new UserQuizRate
            {
                Id = rateId,
                QuizId = "1",
                Rate = rateValue,
                UserId = "userid"
            };

            // Act
            await sut.UpdateRateAsync(rateId, rateValue);

            // Assert
            ratesRepo.Verify(
                m => m.UpdateAsync(It.Is<UserQuizRate>(it => it.Equals(expectedRate))),
                Times.Once);
        }

        [Fact]
        public async Task Update_non_existing_quiz_rate()
        {
            // Arrange
            var ratesRepo = new Mock<IRatesRepository>();
            var sut = new RatesService(ratesRepo.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                ()=>sut.UpdateRateAsync("id", 100));
        }
    }
}
