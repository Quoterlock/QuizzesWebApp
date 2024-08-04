using QuizApp_API.DataAccess.Entities;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IQuizResultsRepository
    {
        Task SaveResultAsync(QuizResult quizResult);
        Task<IEnumerable<QuizResult>> GetByQuizIdAsync(string quizId);
        Task<IEnumerable<QuizResult>> GetByUsernameAsync(string username);
    }
}
