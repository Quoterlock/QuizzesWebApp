using MongoDB.Driver;
using QuizApp_API.DataAccess.Entities;


namespace QuizApp_API.DataAccess.Data
{
    public class MongoDbTmpContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbTmpContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Quiz> Quizzes => _database.GetCollection<Quiz>("Quizzes");
    }
}
