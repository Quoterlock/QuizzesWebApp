using QuizApp_API.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IQuizzesService
    {
        Task<IEnumerable<QuizModel>> GetAsync();
        Task<IEnumerable<QuizModel>> GetAsync(int from, int to);
        Task<QuizModel> GetQuizAsync(string id);
        Task<IEnumerable<QuizListItemModel>> GetTitlesAsync(int from, int to);
        Task<IEnumerable<QuizListItemModel>> GetTitlesAsync();
        Task AddQuizAsync(QuizModel quiz);
    }
}
