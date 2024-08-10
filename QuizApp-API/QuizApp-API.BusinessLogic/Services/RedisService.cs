using StackExchange.Redis;
using System.Text.Json;
namespace QuizApp_API.BusinessLogic.Services
{
    public class RedisService(string connectionString, int expTimeSec)
    {
        private readonly string _connectionString = connectionString;
        private readonly int _expTimeSec = expTimeSec;
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task Delete(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
                IDatabase db = redis.GetDatabase();

                await db.KeyDeleteAsync(key);
                redis.Close();
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<bool> IsExistsAsync(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_connectionString);
                IDatabase db = redis.GetDatabase();

                var result = db.KeyExists(key);

                redis.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool CheckConnection()
        {
            try
            {
                var options = ConfigurationOptions.Parse(_connectionString);
                options.AbortOnConnectFail = false; // Allows it to not wait indefinitely

                using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options))
                {
                    return redis.IsConnected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
