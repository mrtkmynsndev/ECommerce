using System;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces
{
    public interface ICacheService
    {
         Task CacheAsync(string key, object data, TimeSpan cacheTime);

         Task<string> GetCacheAsync(string key);
    }
}