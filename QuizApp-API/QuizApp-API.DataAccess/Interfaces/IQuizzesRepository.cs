using QuizApp_API.DataAccess.Entities;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IQuizzesRepository : IRepository<Quiz>
    {
        Task<IEnumerable<Quiz>> GetRangeAsync(int start, int end);
        Task<IEnumerable<Quiz>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Quiz>> SearchAsync(string value);
    }
}
