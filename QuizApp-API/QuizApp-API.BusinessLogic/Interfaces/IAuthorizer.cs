using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IAuthorizer
    {
        Task<string> Authorize(string username, string password);
    }
}
