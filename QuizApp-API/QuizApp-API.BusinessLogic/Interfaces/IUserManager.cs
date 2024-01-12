using QuizApp_API.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Interfaces
{
    public interface IUserManager
    {
        MockAppUser GetUser(Func<MockAppUser, bool> predicate);
    }
}
