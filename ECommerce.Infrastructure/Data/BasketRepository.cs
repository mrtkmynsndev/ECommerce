using System;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ECommerce.Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasket(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);

            return basket.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id, JsonConvert.SerializeObject(basket), TimeSpan.FromDays(15));

            if (!created) return null;

            return await GetBasketAsync(basket.Id);

        }
    }
}