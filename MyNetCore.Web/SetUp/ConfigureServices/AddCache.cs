using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;

namespace MyNetCore.Web.SetUp
{
    public static class AddCache
    {
        /// <summary>
        /// 添加缓存，MemoryCache或者Redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddMyCache(this IServiceCollection services, IConfiguration config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (config.GetValue<bool>("Cache:UseRedis"))
            {
                services.AddSingleton(typeof(ICacheService), new RedisCacheService(new RedisCacheOptions
                {
                    Configuration = config["Cache:Configuration"],
                    InstanceName = config["Cache:InstanceName"]
                }, config.GetValue<int>("Cache:DefaultDatabase")));
            }
            else
            {
                //services.AddMemoryCache();
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheService, MemoryCacheService>();
            }
        }
    }
}