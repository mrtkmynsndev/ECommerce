using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private int _cacheTime;
        public CachedAttribute(int cacheTime = 5000)
        {
            _cacheTime = cacheTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Specific pipeline request'te bir önceki aksiyon ve bir sonraki aksiyon ne olacağına karar verebilir.
            // Before and After Action Exevuted arasında gerekli işlemleri yapabilriiz.

            var cachedService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var cachedObject = await cachedService.GetCacheAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedObject))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedObject,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next(); // move to controller

            if(executedContext.Result is ObjectResult objectResult)
            {
                await cachedService.CacheAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_cacheTime));
            }   
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path);

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}