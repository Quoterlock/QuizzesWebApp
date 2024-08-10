using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;
using SharpCompress.Common;

namespace QuizApp_API.DataAccess.Repositories
{
    public class QuizResultsRepository(QuizAppDbContext context) 
        : IQuizResultsRepository
    {
        private readonly QuizAppDbContext _context = context;

        public async Task<IEnumerable<QuizResult>> GetByQuizIdAsync(string quizId)
        {
            return _context.Results.Where(e => e.QuizId == quizId);
        }

        public async Task<IEnumerable<QuizResult>> GetByUserIdAsync(string userId)
        {
            return _context.Results.Where(e => e.UserId == userId);
        }

        public async Task RemoveByQuizIdAsync(string quizId)
        {
            var res = _context.Results.Where(e => e.QuizId == quizId);
            _context.Results.RemoveRange(res);
            await _context.SaveChangesAsync();
        }

        public async Task SaveResultAsync(QuizResult quizResult)
        {
            quizResult.Id = Guid.NewGuid().ToString();
            quizResult.TimeStamp = DateTime.Now.ToString();
            _context.Results.Add(quizResult);
            await _context.SaveChangesAsync();
        }
    }
}
