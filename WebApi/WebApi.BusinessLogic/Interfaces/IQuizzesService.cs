using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Models;

namespace WebApi.BusinessLogic.Interfaces
{
    public interface IQuizzesService
    {
        Task<List<QuizModel>> GetAsync();
        Task<List<QuizModel>> GetAsync(int from, int to);
        Task<QuizModel> GetQuizAsync(string id);
        Task<List<QuizListItemModel>> GetTitlesAsync(int from, int to);
        Task<List<QuizListItemModel>> GetTitlesAsync();
        Task AddQuizAsync(QuizModel quiz);
    }
}
