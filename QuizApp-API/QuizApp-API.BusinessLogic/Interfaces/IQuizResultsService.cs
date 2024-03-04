using QuizApp_API.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IQuizResultsService
    {
        Task SaveResultAsync(QuizResultModel quizResult);
        Task<IEnumerable<QuizResultModel>> GetResultsByQuizIdAsync(string quizId);
        Task<IEnumerable<QuizResultModel>> GetResultsByUserIdAsync(string userId);
    }
}
