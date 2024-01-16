using QuizApp_API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetByOwnerIdAsync(string id);
        Task<bool> IsExistsAsync(string ownerId);
    }
}
