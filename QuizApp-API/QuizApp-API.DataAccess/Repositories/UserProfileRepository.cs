using MongoDB.Driver;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace QuizApp_API.DataAccess.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        public MongoDbContext _context;

        public UserProfileRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserProfile entity)
        {
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.OwnerId))
                {
                    try
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        await _context.Profiles.InsertOneAsync(entity);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else throw new Exception("There is no owner_id");
            }
            else throw new ArgumentNullException("profile entity");
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            var filter = Builders<UserProfile>.Filter.Empty; // or any other filter condition
            var projection = new FindOptions<UserProfile, UserProfile>(); // or specify the projection type you need

            return (await _context.Profiles.FindAsync<UserProfile>(filter, projection)).ToEnumerable();
        }

        public async Task<UserProfile> GetByIdAsync(string id)
        {
            return (await _context.Profiles.FindAsync(q => q.Id == id)).FirstOrDefault();
        }

        public async Task<UserProfile> GetByOwnerIdAsync(string id)
        {
            return (await _context.Profiles.FindAsync(q => q.OwnerId == id)).FirstOrDefault();
        }

        public async Task Update(UserProfile entity)
        {
            if (entity != null)
            {
                if (entity.Id != null)
                {
                    if (entity.OwnerId != null)
                    {
                        try
                        {
                            await _context.Profiles.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity);
                            return;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            throw new Exception("Invalid profile entity.");
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistsAsync(string ownerId)
        {
            var profile = await (await _context.Profiles.FindAsync(p => p.OwnerId == ownerId)).FirstAsync();
            return profile != null;
        }
    }
}
