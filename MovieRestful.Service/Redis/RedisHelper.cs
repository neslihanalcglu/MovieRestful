

using Microsoft.EntityFrameworkCore.Storage;
using MovieRestful.Core.Models;
using StackExchange.Redis;

namespace MovieRestful.Service.Redis
{
    public class RedisHelper: IRedisHelper
    {
        public async Task<string> GetKeyAsync(string key)
        {
            var database = await GetRedisDatabase();

            var value = await database.StringGetAsync(key);

            return await Task.FromResult(value);
        }

        public async Task<bool> SetKeyAsync(string key, string value)
        {
            var database = await GetRedisDatabase();

            return await database.StringSetAsync(key, value);
        }

        public async Task<bool> SetKeyAsync(string cacheMovieKey, List<Movie> movies)
        {
            var database = await GetRedisDatabase();
            return await database.StringSetAsync(cacheMovieKey, movies.ToString());
        }

        private async Task<StackExchange.Redis.IDatabase> GetRedisDatabase()
        {
            var config = new ConfigurationOptions
            {
                EndPoints = { "localhost" },
                Ssl = false,
                AbortOnConnectFail = false
            };

            ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync(config);

            return redis.GetDatabase(0);
        }
    }
}
