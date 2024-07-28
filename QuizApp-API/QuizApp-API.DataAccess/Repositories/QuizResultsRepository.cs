using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.DataAccess.Repositories
{
    public class QuizResultsRepository : IQuizResultsRepository
    {
        private readonly QuizAppDbContext _context;
        public QuizResultsRepository(QuizAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<QuizResult>> GetByQuizIdAsync(string quizId)
        {
            return _context.Results.Where(e => e.QuizId == quizId);
;       }

        public async Task<IEnumerable<QuizResult>> GetByUserIdAsync(string userId)
        {
            return _context.Results.Where(e => e.UserId == userId);
        }

        public async Task SaveResultAsync(QuizResult quizResult)
        {
            _context.Results.Add(quizResult);
            await _context.SaveChangesAsync();
        }
    }
}
