using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace TestProject1.ServicesTests
{
    public class QuizResultServiceTests
    {
        private readonly QuizResultsService SUT;
        private readonly MockResultRepository mockRepo;

        public QuizResultServiceTests() 
        {
            mockRepo = new MockResultRepository();
            SUT = new QuizResultsService(mockRepo);
        }


        [Fact]
        public async Task GetByQuizIdTest()
        {
            mockRepo.Seed([new QuizResult { Id = "0", QuizId = "0", UserId = "1", Result = 1 }]);
            var expected = new List<QuizResultModel> { 
                new QuizResultModel { Id = "0", QuizId = "0", UserId = "1", Result = 1 } 
            };
            var actual = await SUT.GetResultsByQuizIdAsync("0");
            Assert.Equal(expected.AsEnumerable(), actual);
        }

        [Fact]
        public async Task SaveResultTest()
        {
            mockRepo.Seed(new List<QuizResult>());
            var expected = new QuizResultModel { Id = "1", QuizId = "1", Result = 1, UserId = "1" };
            await SUT.SaveResultAsync(new QuizResultModel { Id = "1", QuizId = "1", Result = 1, UserId = "1" });
            var result = await SUT.GetResultsByQuizIdAsync("1");
            Assert.Equal(expected, result.Last());
        }

        [Fact]
        public async Task GetByUserId()
        {
            mockRepo.Seed([new QuizResult { Id = "0", QuizId = "0", UserId = "1", Result = 1 }]);
            var expected = new List<QuizResultModel> {
                new QuizResultModel { Id = "0", QuizId = "0", UserId = "1", Result = 1 }
            };
            var actual = await SUT.GetResultsByUserIdAsync("1");
            Assert.Equal(expected.AsEnumerable(), actual);
        }
    }

    class MockResultRepository : IQuizResultsRepository
    {
        private List<QuizResult> quizResults = new List<QuizResult>();

        public MockResultRepository()
        {
            quizResults.Add(new QuizResult { Id = "0", UserId = "1", QuizId = "0", Result = 0 });
        }

        public void Seed(List<QuizResult> entities)
        {
            quizResults = entities;
        }

        public async Task<IEnumerable<QuizResult>> GetByQuizIdAsync(string quizId)
        {
            return quizResults.Where(r => r.QuizId == quizId).AsEnumerable();
        }

        public async Task<IEnumerable<QuizResult>> GetByUserIdAsync(string userId)
        {
            return quizResults.Where(r => r.UserId == userId).AsEnumerable();
        }

        public async Task SaveResultAsync(QuizResult quizResult)
        {
            quizResults.Add(quizResult);
        }
    }
}