using System;
using System.Threading.Tasks;
using ECommerce.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace ECommerce.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _datebase;
        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _datebase = connectionMultiplexer.GetDatabase();
        }

        public async Task CacheAsync(string key, object data, TimeSpan cacheTime)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key can not be null!");

            if (data == null) throw new ArgumentNullException("data can not be null!");

            var serilazedData = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented
            });

            await _datebase.StringSetAsync(key, serilazedData, cacheTime);
        }

        public async Task<string> GetCacheAsync(string key)
        {
            var cachedObject =  await _datebase.StringGetAsync(key);

            if(cachedObject.IsNullOrEmpty) return string.Empty;

            return cachedObject;
        }
    }
}