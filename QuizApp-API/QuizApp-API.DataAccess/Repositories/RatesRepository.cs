using Microsoft.EntityFrameworkCore;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.DataAccess.Repositories
{
    public class RatesRepository : IRatesRepository
    {
        private readonly QuizAppDbContext _context;
        public RatesRepository(QuizAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserQuizRate entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Rates.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
                throw new Exception($"rate with id:{id} doesn't exist");

            _context.Rates.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserQuizRate>> Get(string[] quizIds)
        {
            var entities = _context.Rates
                .Where(e => quizIds.Contains(e.QuizId))
                .ToList();
            return entities;
        }

        public async Task<IEnumerable<UserQuizRate>> GetAllAsync()
        {
            return _context.Rates;
        }

        public async Task<UserQuizRate> GetByIdAsync(string id)
        {
            var entity = await _context.Rates.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
                throw new Exception($"rate with id:{id} doesn't exist");
            return entity;
        }

        public async Task UpdateAsync(UserQuizRate entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByQuizIdAsync(string quizId)
        {
            var res = _context.Rates.Where(e => e.QuizId == quizId);
            _context.Rates.RemoveRange(res);
            await _context.SaveChangesAsync();
        }
    }
}
