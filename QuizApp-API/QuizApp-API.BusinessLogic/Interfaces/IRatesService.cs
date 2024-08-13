using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IRatesService
    {
        Task<List<QuizRateModel>> GetRatesAsync(params string[] quizIds);
        Task AddRateAsync(string quizId, string userId, double rate);
        Task DeleteRate(string rateId);
        Task UpdateRateAsync(string rateId, double rate);
        Task RemoveByQuizIdAsync(string quizId);
    }
}
