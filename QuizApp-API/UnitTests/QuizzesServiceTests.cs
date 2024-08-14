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
        public async Task Add_quiz_text()
        {
            // Arrange
            var quizRepo = new Mock<IQuizzesRepository>();
            var resultsService = new Mock<IQuizResultsService>();
            var ratesService = new Mock<IRatesService>();
            var userProfileService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                quizRepo.Object,
                resultsService.Object,
                ratesService.Object,
                userProfileService.Object);

            var quizModel = new QuizModel
            {
                Title = "Quiz1",
                Author = new UserProfileInfo
                {
                    Owner = new ProfileOwnerInfo { Id = "user-id", Username = "username" }
                },
                Questions = [
                    new QuestionModel {
                        CorrectAnswerIndex = 0, Text = "q1",
                        Options = new List<OptionModel> {
                            new OptionModel { Text = "o1" },
                            new OptionModel { Text = "o2" },
                        }
                    },
                     new QuestionModel {
                        CorrectAnswerIndex = 0, Text = "q2",
                        Options = new List<OptionModel> {
                            new OptionModel { Text = "o1" },
                            new OptionModel { Text = "o2" },
                        }
                    },
                ]
            };

            var expected = new Quiz
            {
                AuthorUserId = "user-id",
                Title = "Quiz1",
                Questions = [
                    new Question { 
                        Title = "q1", CorrectAnswerIndex = 0,
                        Options = [
                            new Option { Text = "o1"},
                            new Option { Text = "o1"},
                        ]
                    },
                    new Question {
                        Title = "q2", CorrectAnswerIndex = 0,
                        Options = [
                            new Option { Text = "o1"},
                            new Option { Text = "o1"},
                        ]
                    }
                ],
            };

            // Act
            await sut.AddQuizAsync(quizModel);

            // Assert
            quizRepo.Verify(
                m => m.AddAsync(It.Is<Quiz>(e=>e.Equals(expected))), 
                Times.Once);
        }

        [Fact]
        public async Task Add_quiz_with_no_author_id_test()
        {
            // Arrange
            var quizRepo = new Mock<IQuizzesRepository>();
            var resultsService = new Mock<IQuizResultsService>();
            var ratesService = new Mock<IRatesService>();
            var userProfileService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                quizRepo.Object,
                resultsService.Object,
                ratesService.Object,
                userProfileService.Object);

            var quizModel = new QuizModel
            {
                Title = "quiz1"
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => sut.AddQuizAsync(quizModel));
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

            var quiz = new QuizModel
            {
                Author = new UserProfileInfo
                {
                    DisplayName = "name",
                    Id = "1",
                    Owner = new ProfileOwnerInfo { Id = "2", Username = "username" }
                }
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => sut.AddQuizAsync(quiz));
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
                ImageId = "1",
                Owner = new ProfileOwnerInfo { Id = authorUserId, Username = "username" },
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
            await Assert.ThrowsAsync<ArgumentException>(
                () => sut.GetByIdAsync(quizId));
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
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => sut.GetByIdAsync(string.Empty));
        }

        [Fact]
        public async Task Get_quiz_list_test()
        {
            string authorUserId = "userId";
            var quizAuthorProfileInfo = new UserProfileInfo
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageId = "1",
                Owner = new ProfileOwnerInfo { Id = authorUserId, Username = "username" },
            };
            var quizAuthorProfileInfo1 = new UserProfileInfo
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageId = "1",
                Owner = new ProfileOwnerInfo { Id = "1", Username = "username" },
            };
            var quizAuthorProfileInfo2 = new UserProfileInfo
            {
                Id = "profileId",
                DisplayName = "Name",
                ImageId = "1",
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

            var mockRepo = new Mock<IQuizzesRepository>();
            mockRepo.Setup(m => m.GetByUserIdAsync(authorId)).ReturnsAsync(quizzes);

            var mockResultsService = new Mock<IQuizResultsService>();

            var mockRatesService = new Mock<IRatesService>();
            mockRatesService.Setup(m => m.GetRatesAsync("1","2")).ReturnsAsync([
                new() { Id = "1", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "2", QuizId="1", Rate=4, UserId="0"},
                new() { Id = "3", QuizId="2", Rate=4, UserId="0"},
                new() { Id = "4", QuizId="2", Rate=8, UserId="0"},
            ]);
            var profileService = new Mock<IUserProfilesService>();
            var userIds = quizzes.Select(q => q.AuthorUserId);
            profileService.Setup(m => m.GetRangeAsync(userIds.ToArray()))
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
            var result = await sut.GetAllTitlesByUserIdAsync(authorId);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Search_by_value_test()
        {
            // Arrange
            var mockRepo = new Mock<IQuizzesRepository>();
            var mockResultsService = new Mock<IQuizResultsService>();
            var mockRatesService = new Mock<IRatesService>();
            var profileService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                mockRepo.Object,
                mockResultsService.Object,
                mockRatesService.Object,
                profileService.Object);

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

            profileService.Setup(m => m.GetRangeAsync("0", "3"))
                .ReturnsAsync([author, author1]);

            Quiz[] quizzes = [
                    new() {Id = "1", Title = "Abs", Questions = [], AuthorUserId = author.Owner.Id},
                    new() {Id = "2", Title = "text-abd-text", AuthorUserId = author1.Owner.Id,Questions = []},
            ];

            mockRepo.Setup(m => m.SearchAsync("ab"))
                .ReturnsAsync(quizzes);
            mockRatesService.Setup(m => m.GetRatesAsync("1", "2"))
                .ReturnsAsync([]);

            var expected = new List<QuizListItemModel>()
            {
                new() { Id = "1", Author = author, Title = "Abs"},
                new() { Id = "2", Author = author1, Title = "text-abd-text"},
            };

            var value = "ab";

            // Act
            var result = await sut.SearchAsync(value);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Get_user_completed_quizzes_count_test()
        {
            // Arrange
            var quizRepo = new Mock<IQuizzesRepository>();
            var resultsService = new Mock<IQuizResultsService>();
            var ratesService = new Mock<IRatesService>();
            var userProfileService = new Mock<IUserProfilesService>();
            var sut = new QuizzesService(
                quizRepo.Object,
                resultsService.Object,
                ratesService.Object,
                userProfileService.Object);

            string userId = "userid";
            var results = new List<QuizResultModel> {
                new QuizResultModel { Id = "1", QuizId = "1", UserProfile = new UserProfileInfo(), 
                    Result = 100, TimeStamp = "" },
                new QuizResultModel { Id = "2", QuizId = "1", UserProfile = new UserProfileInfo(),
                    Result = 90, TimeStamp = "" },
                new QuizResultModel { Id = "3", QuizId = "2", UserProfile = new UserProfileInfo(),
                    Result = 50, TimeStamp = "" },
            };

            resultsService.Setup(m => m.GetResultsByUserIdAsync(userId))
                .ReturnsAsync(results);

            var expected = 2;

            // Act
            var actual = await sut.GetAllUserCompletedAsync(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
