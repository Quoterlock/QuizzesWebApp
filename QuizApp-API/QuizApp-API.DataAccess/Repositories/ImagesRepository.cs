using MongoDB.Driver;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.DataAccess.Repositories
{
    public class ImagesRepository(MongoDbContext db) : IImagesRepository
    {
        private readonly MongoDbContext _db = db;

        public async Task<string> AddImage(byte[] bytes)
        {
            string id = Guid.NewGuid().ToString();
            await _db.Images.InsertOneAsync(new Entities.Image 
            { 
                Bytes = bytes, 
                Id = id
            });
            return id;
        }

        public Task DeleteImage(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetImageAsync(string id)
        {
            var image = (await _db.Images.FindAsync(e => e.Id == id)).First();
            if (image != null)
                return image.Bytes;
            return [];
        }

        public bool IsExists(string imageId)
        {
            return _db.Images.FindSync(e => e.Id == imageId).Any();
        }

        public async Task UpdateImageAsync(string id, byte[] bytes)
        {
            await _db.Images.FindOneAndReplaceAsync(e => e.Id == id, new Entities.Image { Id = id, Bytes = bytes });
        }
    }
}
