using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using MyNetCore.IServices;
using MyNetCore.Services;

namespace MyNetCore.Web.SetUp
{
    public static class AddCache
    {
        /// <summary>
        /// 添加缓存，MemoryCache或者Redis
        /// </summary>
        /// <param name="services"></param>
        public static void AddMyCache(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMemoryCache();

            if (AppSettings.Get<bool>("Cache", "UseRedis"))
            {
                services.AddSingleton(typeof(ICacheServices), new RedisCacheService(new RedisCacheOptions
                {
                    Configuration = AppSettings.Get("Cache", "Configuration"),
                    InstanceName = AppSettings.Get("Cache", "InstanceName")
                }, AppSettings.Get<int>("Cache", "DefaultDatabase")));
            }
            else
            {
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheServices, MemoryCacheService>();
            }



        }
    }
}
