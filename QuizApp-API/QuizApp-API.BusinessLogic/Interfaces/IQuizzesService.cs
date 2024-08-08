using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IQuizzesService
    {
        Task<QuizModel> GetByIdAsync(string id);
        Task<List<QuizListItemModel>> SearchAsync(string value);
        Task<List<QuizListItemModel>> GetTitlesAsync(int from, int to);
        Task<List<QuizListItemModel>> GetTitlesAsync();
        Task RemoveQuizAsync(string id);
        Task AddQuizAsync(QuizModel quiz);
        Task<List<QuizListItemModel>> GetAllTitlesByUserId(string userId);
        Task<int> GetAllUserCompleted(string userId);
    }
}
