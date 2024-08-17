using MongoDB.Driver;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;
using SharpCompress.Common;

namespace QuizApp_API.DataAccess.Repositories
{
    public class QuizRepository : IQuizzesRepository
    {
        public MongoDbContext _context;

        public QuizRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid().ToString();
            quiz.CreationDate = DateTime.Now.ToString();
            await _context.Quizzes.InsertOneAsync(quiz);
        }

        public async Task<IEnumerable<Quiz>> GetRangeAsync(int start, int end)
        {
            int takeCount;
            int count = (int) await _context.Quizzes.EstimatedDocumentCountAsync();

            if (end >= count)
                takeCount = count - start;
            else
                takeCount = end - start;

            return _context.Quizzes
                .Find(_ => true)
                .SortBy(x => x.CreationDate)
                .Skip(start)
                .Limit(takeCount)
                .ToEnumerable();
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            return (await _context.Quizzes.FindAsync(_ => true)).ToEnumerable();
        }

        public async Task<Quiz> GetByIdAsync(string id)
        {
            return (await _context.Quizzes.FindAsync(q => q.Id == id)).FirstOrDefault();
        }

        public Task UpdateAsync(Quiz entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _context.Quizzes.DeleteOneAsync(q => q.Id == id);
            }
            else throw new ArgumentNullException("quiz-id");
        }

        public async Task<IEnumerable<Quiz>> GetByUserIdAsync(string userId)
        {
            var entities = await _context.Quizzes
                .FindAsync(q => q.AuthorUserId == userId);
            return entities.ToEnumerable();
        }

        public async Task<IEnumerable<Quiz>> SearchAsync(string value)
        {
            var entities = await _context.Quizzes
                .FindAsync(q => q.Title.ToLower().Contains(value.ToLower()));
            return entities.ToEnumerable();
        }
    }
}
