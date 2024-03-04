using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

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
            await _context.Quizzes.InsertOneAsync(quiz);
        }

        public async Task<IEnumerable<Quiz>> GetRangeAsync(int start, int end)
        {
            return _context.Quizzes
                .Find(_ => true)
                .Skip(start - 1)
                .Limit(end - start + 1).ToEnumerable();
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            return (await _context.Quizzes.FindAsync(_ => true)).ToEnumerable();
        }

        public async Task<Quiz> GetByIdAsync(string id)
        {
            return (await _context.Quizzes.FindAsync(q => q.Id == id)).FirstOrDefault();
        }

        public Task Update(Quiz entity)
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

        public async Task SaveResultAsync(QuizResult quizResult)
        {
            await _context.Results.InsertOneAsync(quizResult);
        }

        public Task<IEnumerable<Quiz>> GetByAuthorAsync(string authorId)
        {
            throw new NotImplementedException();
        }
    }
}
