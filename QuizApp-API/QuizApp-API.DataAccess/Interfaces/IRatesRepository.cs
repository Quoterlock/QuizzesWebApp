using QuizApp_API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IRatesRepository : IRepository<UserQuizRate>
    {
        Task<List<UserQuizRate>> Get(string[] quizIds);
    }
}
