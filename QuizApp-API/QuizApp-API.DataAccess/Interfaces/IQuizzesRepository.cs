using QuizApp_API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IQuizzesRepository
    {
        Task<List<Quiz>> AsyncGet();
        Task<List<Quiz>> AsyncGet(int start, int end);
        Task<Quiz?> AsyncGet(string id);
        Task AddAsync(Quiz quiz);
    }
}
