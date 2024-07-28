using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IQuizzesService
    {
        Task<QuizModel> GetByIdAsync(string id);
        Task<IEnumerable<QuizListItemModel>> GetTitlesAsync(int from, int to);
        Task<IEnumerable<QuizListItemModel>> GetTitlesAsync();
        Task RemoveQuizAsync(string id);
        Task AddQuizAsync(QuizModel quiz);
        Task<IEnumerable<QuizListItemModel>> GetAllTitlesByAuthorId(string profileId);
    }
}
