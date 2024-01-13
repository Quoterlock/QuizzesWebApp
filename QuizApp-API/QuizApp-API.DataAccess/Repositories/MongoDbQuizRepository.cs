using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.DataAccess.Repositories
{
    public class MongoDbQuizRepository : IQuizzesRepository
    {
        public MongoDbTmpContext _context;
        public MongoDbQuizRepository()
        {
            _context = new MongoDbTmpContext("mongodb://localhost:27017", "QuizAppDb");
        }
        public async Task AddAsync(Quiz quiz)
        {
            await _context.Quizzes.InsertOneAsync(quiz);
        }

        public async Task<List<Quiz>> AsyncGet()
        {
            return (await _context.Quizzes.FindAsync(_ => true)).ToList();
        }

        public async Task<List<Quiz>> AsyncGet(int start, int end)
        {
            return await _context.Quizzes
                .Find(_ => true)
                .Skip(start - 1)
                .Limit(end - start + 1)
                .ToListAsync();
        }

        public async Task<Quiz?> AsyncGet(string id)
        {
            return (await _context.Quizzes.FindAsync(q=>q.Id == id)).FirstOrDefault();
        }
    }
}
