using StackExchange.Redis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ZstdSharp;

namespace QuizApp_API.BusinessLogic.Services
{
    public class RedisService(string connectionString, int expTimeSec)
    {
        private readonly string _connectionString = connectionString;
        private readonly int _expTimeSec = expTimeSec;
        public async Task<T?> Get<T>(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
            IDatabase db = redis.GetDatabase();
            
            string jsonString = await db.StringGetAsync(key);
            if (jsonString == null)
            {
                redis.Close();
                return default;
            }
            var item = JsonSerializer.Deserialize<T>(jsonString);
            redis.Close();
            return item;
        }

        public async Task Set<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
            IDatabase db = redis.GetDatabase();

            string jsonString = JsonSerializer.Serialize(value);
            var time = TimeSpan.FromSeconds(_expTimeSec); // 5 mins
            await db.StringSetAsync(key, jsonString, time);

            redis.Close();
        }

        public async Task Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
            IDatabase db = redis.GetDatabase();

            await db.KeyDeleteAsync(key);

            redis.Close();
        }

        public async Task<bool> IsExistsAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
            IDatabase db = redis.GetDatabase();

            var result = db.KeyExists(key);

            redis.Close();
            return result;
        }
    }
}
