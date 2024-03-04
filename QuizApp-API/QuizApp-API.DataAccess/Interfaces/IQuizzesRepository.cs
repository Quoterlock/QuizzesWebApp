using QuizApp_API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IQuizzesRepository : IRepository<Quiz>
    {
        Task<IEnumerable<Quiz>> GetRangeAsync(int start, int end);
        Task<IEnumerable<Quiz>> GetByAuthorAsync(string authorId);
    }
}
