using QuizApp_API.DataAccess.Entities;
namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetByOwnerIdAsync(string id);
        Task<IEnumerable<UserProfile>> GetRangeByOwnerIdsAsync(string[] ownerIds);
        Task<bool> IsExistsAsync(string ownerId);
    }
}
