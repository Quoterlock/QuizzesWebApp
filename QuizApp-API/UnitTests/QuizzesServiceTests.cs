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
        public async Task Get_user_completed_quizzes_count_test()
        {
            // Arrange
            string username = "user";
            var userProfile = new UserProfileInfo { 
                DisplayName = "Name",
                Id = "profileId", 
                ImageBytes = [], 
                Owner = new ProfileOwnerInfo { 
                    Id = "userId", Username = username } 
            };

            var results = new List<QuizResultModel>
            {
                new() { Id = "1", QuizId = "2", Result = 4, UserProfile = userProfile },
                new() { Id = "2", QuizId = "2", Result = 4, UserProfile = userProfile },
                new() { Id = "3", QuizId = "3", Result = 4, UserProfile = userProfile },
                new() { Id = "4", QuizId = "4", Result = 4, UserProfile = userProfile },
                new() { Id = "5", QuizId = "4", Result = 4, UserProfile = userProfile },
            };

            var profilesService = new Mock<IUserProfilesService>();
            var ratesService = new Mock<IRatesService>();
            var mockRepo = new Mock<IQuizzesRepository>();

            var resultsService = new Mock<IQuizResultsService>();
            resultsService.Setup(m => m.GetResultsByUserIdAsync(username))
                .ReturnsAsync(results);

            var sut = new QuizzesService(
                mockRepo.Object, 
                resultsService.Object, 
                ratesService.Object, 
                profilesService.Object);

            int expected = 3;

            // Act
            var actual = await sut.GetAllUserCompleted(username);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_full_quiz_info_by_id_test()
        {
            // Arrange
            var quizId = "1";
            var authorUserId = "userId";
            var mockQuizzesRepo = new Mock<IQuizzesRepository>();
            mockQuizzesRepo
                .Setup(m => m.GetByIdAsync(quizId))
                .ReturnsAsync(new Quiz
                {
                    Title = "Quiz",
                    AuthorUserId = authorUserId,
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
            var profilesService = new Mock<IUserProfilesService>();
            var authorProfile = new UserProfileInfo()
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageBytes = [],
                Owner = new ProfileOwnerInfo { Id = authorUserId, Username = "username"  },
            };
            profilesService.Setup(m => m.GetRangeAsync(authorUserId))
                .ReturnsAsync([authorProfile]);

            var sut = new QuizzesService(
                mockQuizzesRepo.Object, 
                mockResultsService.Object, 
                mockRatesService.Object,
                profilesService.Object);

            var expected = new QuizModel
            {
                Author = authorProfile,
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
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizzesService(
                mockRepo.Object, 
                mockResultsService.Object, 
                mockRatesService.Object,
                profilesService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetByIdAsync(quizId));
        }

        [Fact]
        public async Task Get_quiz_with_no_id_test()
        {
            // Arrange
            var mockRepo = new Mock<IQuizzesRepository>();
            var mockResultsService = new Mock<IQuizResultsService>();
            var mockRatesService = new Mock<IRatesService>();
            var profilesService = new Mock<IUserProfilesService>();

            var sut = new QuizzesService(
                mockRepo.Object,
                mockResultsService.Object,
                mockRatesService.Object,
                profilesService.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetByIdAsync(string.Empty));
        }

        [Fact]
        public async Task Get_quiz_list_test()
        {
            string authorUserId = "userId";
            var quizAuthorProfileInfo = new UserProfileInfo
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageBytes = [],
                Owner = new ProfileOwnerInfo { Id = authorUserId, Username = "username" },
            };
            var quizAuthorProfileInfo1 = new UserProfileInfo
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageBytes = [],
                Owner = new ProfileOwnerInfo { Id = "1", Username = "username" },
            };
            var quizAuthorProfileInfo2 = new UserProfileInfo
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageBytes = [],
                Owner = new ProfileOwnerInfo { Id = "2", Username = "username" },
            };
            // Arrange
            Quiz[] quizzes = [
                    new() {Id = "1", AuthorUserId = quizAuthorProfileInfo.Owner.Id, Title = "Quiz1", Questions = []},
                    new() {Id = "2", AuthorUserId = quizAuthorProfileInfo1.Owner.Id, Title = "Quiz2",Questions = []},
                    new() {Id = "3", AuthorUserId = quizAuthorProfileInfo2.Owner.Id, Title = "Quiz3",Questions = []},
                    new() {Id = "4", AuthorUserId = quizAuthorProfileInfo2.Owner.Id, Title = "Quiz4",Questions = []},
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


            var profilesService = new Mock<IUserProfilesService>();
            var userIds = new List<UserProfileInfo>() { quizAuthorProfileInfo, quizAuthorProfileInfo1, quizAuthorProfileInfo2 }.Select(e => e.Owner.Id);
            profilesService.Setup(m => m.GetRangeAsync(userIds.ToArray()))
                .ReturnsAsync([quizAuthorProfileInfo, quizAuthorProfileInfo1, quizAuthorProfileInfo2]);

            var sut = new QuizzesService(
                mockRepo.Object, 
                mockResultsService.Object, 
                mockRatesService.Object,
                profilesService.Object);
            
            var expected = new List<QuizListItemModel>()
            {
                new() { Id = "1", Author = quizAuthorProfileInfo, Title = "Quiz1", Rate = 4},
                new() { Id = "2", Author = quizAuthorProfileInfo1, Title = "Quiz2", Rate = 6},
                new() { Id = "3", Author = quizAuthorProfileInfo2, Title = "Quiz3", Rate = 10},
                new() { Id = "4", Author = quizAuthorProfileInfo2, Title = "Quiz4", Rate = 0},
            };

            // Act
            var actual = await sut.GetTitlesAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_quiz_by_author_id_list_test()
        {
            // Arrange
            string authorId = "1";

            var author = new UserProfileInfo
            {
                DisplayName = "Name",
                Id = "1",
                Owner = new ProfileOwnerInfo { Id = "0", Username = "username" }
            };
            var author1 = new UserProfileInfo
            {
                DisplayName = "Name",
                Id = "2",
                Owner = new ProfileOwnerInfo { Id = "3", Username = "username1" }
            };

            Quiz[] quizzes = [
                    new() {Id = "1", Title = "Quiz1", Questions = [], AuthorUserId = author.Owner.Id},
                    new() {Id = "2", AuthorUserId = author1.Owner.Id, Title = "Quiz2",Questions = []},
                ];
            var ids = quizzes
                .Where(q=>q.AuthorUserId == authorId)
                .Select(q => q.Id).ToArray();

            var mockRepo = new Mock<IQuizzesRepository>();
            mockRepo.Setup(m => m.GetByUserIdAsync(authorId)).ReturnsAsync(quizzes);

            var mockResultsService = new Mock<IQuizResultsService>();

            var mockRatesService = new Mock<IRatesService>();
            mockRatesService.Setup(m => m.GetRatesAsync(ids)).ReturnsAsync([
                new() { Id = "1", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "2", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "3", QuizId="2", Rate=4, UserId="0"},
                new() { Id = "4", QuizId="2", Rate=8, UserId="0"},
            ]);
            var profileService = new Mock<IUserProfilesService>();
            var userIds = quizzes.Select(q => q.AuthorUserId);
            profileService.Setup(m=>m.GetRangeAsync(userIds.ToArray()))
                .ReturnsAsync([author, author1]);

            var sut = new QuizzesService(
                mockRepo.Object,
                mockResultsService.Object, 
                mockRatesService.Object,
                profileService.Object);

            var expected = new List<QuizListItemModel>()
            {
                new() { Id = "1", Author = author, Title = "Quiz1", Rate = 4},
                new() { Id = "2", Author = author1, Title = "Quiz2", Rate = 6},
            };

            // Act
            var result = await sut.GetAllTitlesByUserId(authorId);

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
            var profilesService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object,
                profilesService.Object);
            var id = "1";

            // Act
            await sut.RemoveQuizAsync(id);

            // Assert
            repoMock.Verify(m => m.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task Remove_quiz_with_empty_id_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var profilesService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object,
                profilesService.Object);
            string id = string.Empty;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.RemoveQuizAsync(id));

        }

        [Fact]
        public async Task Add_quiz_test()
        {
            // Arrange
            var repoMock = new Mock<IQuizzesRepository>();
            var ratesService = new Mock<IRatesService>();
            var resultsService = new Mock<IQuizResultsService>();
            var profilesService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object,
                profilesService.Object);

            var quizModel = new QuizModel()
            {
                Author = new UserProfileInfo {
                    DisplayName = "Name",
                    Id = "1",
                    ImageBytes = [],
                    Owner = new ProfileOwnerInfo
                    {
                        Id = "2",
                        Username = "username"
                    }
                },
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
                AuthorUserId = "2",
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

            return expected.AuthorUserId == actual.AuthorUserId &&
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
            var profilesService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object,
                profilesService.Object);
            var quiz = new QuizModel { 
                Author = new UserProfileInfo
                {
                    DisplayName = "name",
                    Id = "1",
                    Owner = new ProfileOwnerInfo { Id = "2", Username = "username" }
                }
            };
            
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
            var profilesService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                repoMock.Object,
                resultsService.Object,
                ratesService.Object,
                profilesService.Object);
            var quiz = new QuizModel
            {
                Title = "title"
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddQuizAsync(quiz));
        }
    }
}
