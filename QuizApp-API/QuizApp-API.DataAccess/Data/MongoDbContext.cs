using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizApp_API.DataAccess.Entities;


namespace QuizApp_API.DataAccess.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(string connectionString)
        {
            var args = connectionString.Split(';'); // get all arguments
            if (args.Length == 2)
            {
                var client = new MongoClient(args[0]);
                _database = client.GetDatabase(args[1]);
            }
            else throw new Exception(
                "connection string isn't properly formatted (<mongo_db_connection>;<db_name>)"
            );
        }

        public IMongoCollection<Quiz> Quizzes => _database.GetCollection<Quiz>("Quizzes");
        public IMongoCollection<UserProfile> Profiles => _database.GetCollection<UserProfile>("UserProfiles");
        public IMongoCollection<QuizResult> Results => _database.GetCollection<QuizResult>("QuizResults");
    }
}
