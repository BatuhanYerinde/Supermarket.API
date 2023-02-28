using Newtonsoft.Json;
using StackExchange.Redis;
using Supermarket.API.Domain.Services.Cache;

namespace Supermarket.API.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisCon;
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redisCon)
        {
            _redisCon = redisCon;
            _db = redisCon.GetDatabase();
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public async Task<object> RemoveDataAsync(string key)
        {
            bool _isKeyExist = await _db.KeyExistsAsync(key);
            if (_isKeyExist)
            {
                return await _db.KeyDeleteAsync(key);
            }
            return false;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
    }
}
