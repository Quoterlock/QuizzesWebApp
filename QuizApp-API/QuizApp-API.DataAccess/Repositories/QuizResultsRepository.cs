using MongoDB.Driver;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.DataAccess.Repositories
{
    public class QuizResultsRepository : IQuizResultsRepository
    {
        private readonly MongoDbContext _context;
        public QuizResultsRepository(MongoDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<QuizResult>> GetByQuizIdAsync(string quizId)
        {
            var filter = Builders<QuizResult>.Filter.Eq(q => q.QuizId, quizId);
            var projection = new FindOptions<QuizResult, QuizResult>(); // specify the projection type you need
            return (await _context.Results.FindAsync<QuizResult>(filter, projection)).ToEnumerable();
;       }

        public async Task<IEnumerable<QuizResult>> GetByUserIdAsync(string userId)
        {
            var filter = Builders<QuizResult>.Filter.Eq(q => q.UserId, userId);
            var projection = new FindOptions<QuizResult, QuizResult>(); // or specify the projection type you need
            return (await _context.Results.FindAsync<QuizResult>(filter, projection)).ToEnumerable();
        }

        public async Task SaveResultAsync(QuizResult quizResult)
        {
            quizResult.Id = Guid.NewGuid().ToString();  
            await _context.Results.InsertOneAsync(quizResult);
        }
    }
}
