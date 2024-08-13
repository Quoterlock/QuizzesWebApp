using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IQuizResultsService
    {
        Task SaveResultAsync(string quizId, string userId, int result);
        Task<IEnumerable<QuizResultModel>> GetResultsByQuizIdAsync(string quizId);
        Task<IEnumerable<QuizResultModel>> GetResultsByUserIdAsync(string userId);
        Task RemoveByQuizIdAsync(string quizId);
    }
}
