using QuizApp_API.DataAccess.Entities;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IRatesRepository : IRepository<UserQuizRate>
    {
        Task<List<UserQuizRate>> Get(string[] quizIds);
        Task RemoveByQuizIdAsync(string quizId);

    }
}
