using Moq;
using QuizApp_API.BusinessLogic;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace UnitTests
{
    public class QuizzesServiceTests
    {
        [Fact]
        public async Task Get_full_quiz_info_by_id_test()
        {
            // Arrange
            var quizId = "1";
            var mockRepo = new Mock<IQuizzesRepository>();
            mockRepo
                .Setup(m => m.GetByIdAsync(quizId))
                .ReturnsAsync(new Quiz
                {
                    Title = "Quiz",
                    AuthorId = "0",
                    AuthorName = "user",
                    Id = quizId,
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Title = "Q",
                            CorrectAnswerIndex = 0,
                            Options = new List<Option>
                            {
                                new Option { Text = "option" }
                            }
                        }
                    }
                });

            var mockResultsService = new Mock<IQuizResultsService>();
            mockResultsService
                .Setup(m => m.GetResultsByQuizIdAsync(quizId))
                .ReturnsAsync([new QuizResultModel { Id = "3", QuizId = quizId }]);

            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object);
            var expected = new QuizModel
            {
                Author = "user",
                AuthorId = "0",
                Id = "1",
                Rate = 0,
                Title = "Quiz",
                Results = [new QuizResultModel { Id = "3", QuizId = quizId }],
                Questions = new[]
                {
                    new QuestionModel
                    {
                        Text = "Q",
                        CorrectAnswerIndex = 0,
                        Options = new List<OptionModel>
                        {
                            new OptionModel { Text = "option" }
                        }
                    }
                }
            };

            // Act
            var result = await sut.GetByIdAsync("1");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Get_not_existing_quiz_test()
        {
            // Arrange
            string quizId = "1";
            var mockRepo = new Mock<IQuizzesRepository>();
            var mockResultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object);

            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.GetByIdAsync(quizId));
        }

        [Fact]
        public async Task Get_quiz_with_null_id_test()
        {
            // Arrange
            var mockRepo = new Mock<IQuizzesRepository>();
            var mockResultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetByIdAsync(null));
        }

        [Fact]
        public async Task Get_quiz_list_test()
        {
            // Arrange
            var mockRepo = new Mock<IQuizzesRepository>();
            mockRepo.Setup(m => m.GetAllAsync()).ReturnsAsync([
                    new Quiz {Id = "1", AuthorId = "2", AuthorName = "Name", Title = "Quiz1", Questions = []},
                    new Quiz {Id = "2", AuthorId = "3", AuthorName = "Name1", Title = "Quiz2",Questions = []},
                    new Quiz {Id = "3", AuthorId = "4", AuthorName = "Name2", Title = "Quiz3",Questions = []},
                    new Quiz {Id = "4", AuthorId = "5", AuthorName = "Name3", Title = "Quiz4",Questions = []},
                    new Quiz {Id = "5", AuthorId = "6", AuthorName = "Name4", Title = "Quiz5",Questions = []},
                    new Quiz {Id = "6", AuthorId = "7", AuthorName = "Name5", Title = "Quiz6",Questions = []},
                ]);
            var mockResultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object);
            var expected = new List<QuizListItemModel>()
            {
                new QuizListItemModel { Id = "1", Author = "Name", AuthorId = "2", Title = "Quiz1", Rate = 0},
                new QuizListItemModel { Id = "2", Author = "Name1", AuthorId = "3", Title = "Quiz2", Rate = 1},
                new QuizListItemModel { Id = "3", Author = "Name2", AuthorId = "4", Title = "Quiz3", Rate = 2},
                new QuizListItemModel { Id = "4", Author = "Name3", AuthorId = "5", Title = "Quiz4", Rate = 3},
                new QuizListItemModel { Id = "5", Author = "Name4", AuthorId = "6", Title = "Quiz5", Rate = 4},
                new QuizListItemModel { Id = "6", Author = "Name5", AuthorId = "7", Title = "Quiz6", Rate = 5},
            };

            // Act
            var result = await sut.GetTitlesAsync();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
