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
            var mockQuizzesRepo = new Mock<IQuizzesRepository>();
            mockQuizzesRepo
                .Setup(m => m.GetByIdAsync(quizId))
                .ReturnsAsync(new Quiz
                {
                    Title = "Quiz",
                    AuthorId = "0",
                    AuthorName = "user",
                    Id = quizId,
                    Questions = [
                        new Question
                        {
                            Title = "Q",
                            CorrectAnswerIndex = 0,
                            Options = [ new Option { Text = "option" } ]
                        }]
                });

            var mockResultsService = new Mock<IQuizResultsService>();
            mockResultsService
                .Setup(m => m.GetResultsByQuizIdAsync(quizId))
                .ReturnsAsync([new QuizResultModel { Id = "3", QuizId = quizId }]);

            var mockRatesService = new Mock<IRatesService>();
            mockRatesService.Setup(m => m.GetRatesAsync(quizId))
                .ReturnsAsync(
                [
                    new() {Id = "1", Rate = 15, QuizId = quizId, UserId = "0"},
                    new() {Id = "2", Rate = 5, QuizId = quizId, UserId = "0"},
                    new() {Id = "3", Rate = 10, QuizId = "another", UserId = "0"},
                    new() {Id = "4", Rate = 10, QuizId = "another", UserId = "0"},
                ]);

            var sut = new QuizzesService(mockQuizzesRepo.Object, mockResultsService.Object, mockRatesService.Object);
            var expected = new QuizModel
            {
                Author = "user",
                AuthorId = "0",
                Id = "1",
                Rate = 10,
                Title = "Quiz",
                Results = [new QuizResultModel { Id = "3", QuizId = quizId }],
                Questions = [
                    new QuestionModel
                    {
                        Text = "Q",
                        CorrectAnswerIndex = 0,
                        Options =
                        [
                            new OptionModel { Text = "option" }
                        ]
                    }]
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
            var mockRatesService = new Mock<IRatesService>();

            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object, mockRatesService.Object);

            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.GetByIdAsync(quizId));
        }

        [Fact]
        public async Task Get_quiz_with_null_id_test()
        {
            // Arrange
            var mockRepo = new Mock<IQuizzesRepository>();
            var mockResultsService = new Mock<IQuizResultsService>();
            var mockRatesService = new Mock<IRatesService>();

            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object, mockRatesService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetByIdAsync(null));
        }

        [Fact]
        public async Task Get_quiz_list_test()
        {
            // Arrange
            Quiz[] quizzes = [
                    new() {Id = "1", AuthorId = "2", AuthorName = "Name", Title = "Quiz1", Questions = []},
                    new() {Id = "2", AuthorId = "3", AuthorName = "Name1", Title = "Quiz2",Questions = []},
                    new() {Id = "3", AuthorId = "4", AuthorName = "Name2", Title = "Quiz3",Questions = []},
                    new() {Id = "4", AuthorId = "5", AuthorName = "Name3", Title = "Quiz4",Questions = []},
                ];
            var ids = quizzes.Select(q => q.Id).ToArray();

            var mockRepo = new Mock<IQuizzesRepository>();
            mockRepo.Setup(m => m.GetAllAsync()).ReturnsAsync(quizzes);

            var mockResultsService = new Mock<IQuizResultsService>();
            
            var mockRatesService = new Mock<IRatesService>();
            mockRatesService.Setup(m => m.GetRatesAsync(ids)).ReturnsAsync([ 
                new() { Id = "1", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "2", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "3", QuizId="2", Rate=4, UserId="0"},
                new() { Id = "4", QuizId="2", Rate=8, UserId="0"},
                new() { Id = "5", QuizId="3", Rate=10, UserId="0"},
            ]);
            
            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object, mockRatesService.Object);
            var expected = new List<QuizListItemModel>()
            {
                new() { Id = "1", Author = "Name", AuthorId = "2", Title = "Quiz1", Rate = 4},
                new() { Id = "2", Author = "Name1", AuthorId = "3", Title = "Quiz2", Rate = 6},
                new() { Id = "3", Author = "Name2", AuthorId = "4", Title = "Quiz3", Rate = 10},
                new() { Id = "4", Author = "Name3", AuthorId = "5", Title = "Quiz4", Rate = 0},
            };

            // Act
            var result = await sut.GetTitlesAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Get_quiz_by_author_id_list_test()
        {
            // Arrange
            string authorId = "1";
            Quiz[] quizzes = [
                    new() {Id = "1", AuthorId = authorId, AuthorName = "Name", Title = "Quiz1", Questions = []},
                    new() {Id = "2", AuthorId = authorId, AuthorName = "Name1", Title = "Quiz2",Questions = []},
                ];
            var ids = quizzes
                .Where(q=>q.AuthorId == authorId)
                .Select(q => q.Id).ToArray();

            var mockRepo = new Mock<IQuizzesRepository>();
            mockRepo.Setup(m => m.GetByAuthorAsync(authorId)).ReturnsAsync(quizzes);

            var mockResultsService = new Mock<IQuizResultsService>();

            var mockRatesService = new Mock<IRatesService>();
            mockRatesService.Setup(m => m.GetRatesAsync(ids)).ReturnsAsync([
                new() { Id = "1", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "2", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "3", QuizId="2", Rate=4, UserId="0"},
                new() { Id = "4", QuizId="2", Rate=8, UserId="0"},
            ]);

            var sut = new QuizzesService(mockRepo.Object, mockResultsService.Object, mockRatesService.Object);
            var expected = new List<QuizListItemModel>()
            {
                new() { Id = "1", Author = "Name", AuthorId = authorId, Title = "Quiz1", Rate = 4},
                new() { Id = "2", Author = "Name1", AuthorId = authorId, Title = "Quiz2", Rate = 6},
            };

            // Act
            var result = await sut.GetAllTitlesByAuthorId(authorId);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Remove_quiz_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object);
            var id = "1";

            // Act
            await sut.RemoveQuizAsync(id);

            // Assert
            repoMock.Verify(m => m.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task Add_quiz_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(
                repoMock.Object, 
                resultsService.Object, 
                ratesService.Object);
            var quizModel = new QuizModel()
            {
                Author = "name",
                AuthorId = "1",
                Title = "title",
                Rate = 1,
                Results = [],
                Questions = [
                    new QuestionModel {
                        Options = [ new OptionModel { Text = "option" }],
                        CorrectAnswerIndex = 1,
                        Text = "Q1"
                    },
                    new QuestionModel {
                        Options = [ new OptionModel { Text = "option1" }],
                        CorrectAnswerIndex = 1,
                        Text = "Q2"
                    },
                ],
            };
            var expected = new Quiz
            {
                AuthorId = "1",
                AuthorName = "name",
                Title = "title",
                Questions = [
                    new Question {
                        Title = "Q1",
                        CorrectAnswerIndex = 1,
                        Options = [
                            new Option { Text = "option" }
                        ]
                    },
                    new Question {
                        Title = "Q2",
                        CorrectAnswerIndex = 1,
                        Options = [
                            new Option { Text = "option1"}
                        ]
                    },
                ]
            };

            // Act
            await sut.AddQuizAsync(quizModel);

            // Assert
            repoMock.Verify(
                x => x.AddAsync(It.Is<Quiz>(
                    e => MatchesQuizIgnoringIdAndCreationDate(expected, e))),
                Times.Once());
        }

        private static bool MatchesQuizIgnoringIdAndCreationDate(Quiz expected, Quiz actual)
        {
            if (expected == null || actual == null)
                return false;

            return expected.AuthorId == actual.AuthorId &&
                   expected.AuthorName == actual.AuthorName &&
                   expected.Title == actual.Title &&
                   expected.Questions.SequenceEqual(actual.Questions);
        }

        [Fact]
        public async Task Add_quiz_with_no_title_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object);
            var quiz = new QuizModel { Author = "1", AuthorId = "1" };
            
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddQuizAsync(quiz));
        }

        [Fact]
        public async Task Add_quiz_with_no_authorId_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object);
            var quiz = new QuizModel { Title = "1", Author = "1" };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddQuizAsync(quiz));
        }

        [Fact]
        public async Task Add_quiz_with_no_author_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object);
            var quiz = new QuizModel { Title = "1", AuthorId = "1" };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddQuizAsync(quiz));
        }
    }
}
