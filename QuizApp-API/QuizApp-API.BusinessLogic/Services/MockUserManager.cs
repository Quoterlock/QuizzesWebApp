using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Services
{
    public class MockUserManager : IUserManager
    {
        public MockAppUser GetUser(Func<MockAppUser, bool> predicate)
        {
            var all = GetAll();
            return all.FirstOrDefault(predicate) ?? throw new Exception("User don't found");
        }

        private List<MockAppUser> GetAll()
        {
            return new List<MockAppUser> {
                new MockAppUser { Id="1", Username="user", Password="1234" },
                new MockAppUser { Id="2", Username="admin", Password="1234" }
            };
        }
    }
}
