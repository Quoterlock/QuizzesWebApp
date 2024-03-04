using QuizApp_API.BusinessLogic;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;
using System.Configuration;

namespace UnitTests.ServicesTests
{
    public class QuizzesServiceTests
    {
        private QuizzesService SUT;
        private MockQuizRepository mockRepo;
        public QuizzesServiceTests() 
        {
            mockRepo = new MockQuizRepository();
            SUT = new QuizzesService(mockRepo);
        }

        [Fact]
        public async Task GetQuizByIdTest()
        {
            mockRepo.Seed([new Quiz { AuthorId = "1", AuthorName = "Author", Id = "1", Questions = new List<Question>(), Title = "title" }]);
            var expected = new QuizModel { AuthorId = "1", Author = "Author", Id = "1", Questions = new QuestionModel[0], Title = "title" };
            var actual = await SUT.GetByIdAsync("1");
            Assert.Equal(expected, actual);
        }
    }

    class MockQuizRepository : IQuizzesRepository
    {
        private List<Quiz> _data;
        public MockQuizRepository() 
        {
            _data = new List<Quiz>();
        }

        public void Seed(List<Quiz> data)
        {
            _data = data;
        }

        public List<Quiz> GetState()
        {
            return _data;
        }
        public async Task AddAsync(Quiz entity)
        {
            _data.Add(entity);
        }

        public async Task DeleteAsync(Quiz entity)
        {
            _data.Remove(entity);
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            return _data.AsEnumerable();
        }

        public async Task<IEnumerable<Quiz>> GetByAuthorAsync(string authorId)
        {
            return _data.Where(e => e.AuthorId == authorId).AsEnumerable();
        }

        public async Task<Quiz> GetByIdAsync(string id)
        {
            return _data.FirstOrDefault(e => e.Id == id);
        }

        public async Task<IEnumerable<Quiz>> GetRangeAsync(int start, int end)
        {
            return _data.GetRange(start, end - start);
        }

        public Task Update(Quiz entity)
        {
            throw new NotImplementedException();
        }
    }
}
