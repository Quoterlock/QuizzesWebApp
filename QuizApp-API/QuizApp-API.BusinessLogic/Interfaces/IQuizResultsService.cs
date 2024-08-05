using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IQuizResultsService
    {
        Task SaveResultAsync(QuizResultModel quizResult);
        Task<IEnumerable<QuizResultModel>> GetResultsByQuizIdAsync(string quizId);
        Task<IEnumerable<QuizResultModel>> GetResultsByUserIdAsync(string userId);
    }
}
