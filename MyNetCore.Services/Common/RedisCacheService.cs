using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using Newtonsoft.Json;

namespace MyNetCore.Services
{
    /// <summary>
    /// MemoryCache 实现
    /// </summary>
    public class RedisCacheService : ICacheServices
    {
        protected IDatabase _cache;
        private ConnectionMultiplexer _connection;
        private readonly string _instance;

        /// <summary>
        /// redis对象实例化
        /// </summary>
        /// <param name="options">配置</param>
        /// <param name="database">操作的数据库</param>
        public RedisCacheService(RedisCacheOptions options, int database = 0)
        {
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
            _instance = options.InstanceName;
        }

        #region 是否存在

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            return _cache.KeyExists(GetKeyForRedis(key));
        }

        /// <summary>
        /// 验证缓存项是否存在（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            return _cache.KeyExistsAsync(GetKeyForRedis(key));
        }

        #endregion

        #region 添加缓存

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, object value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 添加缓存，指定过期时间
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresTime">指定过期时间</param>
        /// <returns></returns>
        public bool Add(string key, object value, DateTime expiresTime)
        {
            if (expiresTime <= DateTime.Now) throw new Exception("过期时间不能小于或等于当前时间");

            var expiresIn = expiresTime - DateTime.Now;
            return Add(key, value, expiresIn, false);
        }

        /// <summary>
        /// 添加缓存，指定过期时间
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresTime">指定过期时间</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, DateTime expiresTime)
        {
            if (expiresTime <= DateTime.Now) throw new Exception("过期时间不能小于或等于当前时间");

            var expiresIn = expiresTime - DateTime.Now;
            return await AddAsync(key, value, expiresIn, false);
        }

        /// <summary>
        /// 添加缓存，指定多少秒后过期
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSecond">多少秒后过期</param>
        /// <returns></returns>
        public bool Add(string key, object value, int expiresSecond)
        {
            if (expiresSecond < 1) throw new Exception("秒必须大于0");
            var expiresIn = DateTime.Now.AddSeconds(expiresSecond) - DateTime.Now;
            return Add(key, value, expiresIn, false);
        }

        /// <summary>
        /// 添加缓存，指定多少秒后过期
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSecond">多少秒后过期</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, int expiresSecond)
        {
            if (expiresSecond < 1) throw new Exception("秒必须大于0");
            var expiresIn = DateTime.Now.AddSeconds(expiresSecond) - DateTime.Now;
            return await AddAsync(key, value, expiresIn, false);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">Redis不支持滑动过期</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiressAbsoulte);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">Redis不支持滑动过期</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiressAbsoulte);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
        }

        #endregion

        #region 删除缓存

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            return _cache.KeyDelete(GetKeyForRedis(key));
        }

        /// <summary>
        /// 删除缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            return _cache.KeyDeleteAsync(GetKeyForRedis(key));
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            keys.ToList().ForEach(item => Remove(item));
        }

        /// <summary>
        /// 批量删除缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            await Task.Run(() => keys.ToList().ForEach(item => RemoveAsync(item)));
        }

        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            var value = _cache.StringGet(GetKeyForRedis(key));

            if (!value.HasValue) return null;

            return JsonConvert.DeserializeObject<string>(value);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            var value = await _cache.StringGetAsync(GetKeyForRedis(key));

            if (!value.HasValue) return null;

            return JsonConvert.DeserializeObject<string>(value);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            var value = _cache.StringGet(GetKeyForRedis(key));

            if (!value.HasValue) return null;
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            var value = await _cache.StringGetAsync(GetKeyForRedis(key));

            if (!value.HasValue) return null;

            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object GetObj(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            var value = _cache.StringGet(GetKeyForRedis(key));

            if (!value.HasValue) return null;

            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<object> GetObjAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            var value = await _cache.StringGetAsync(GetKeyForRedis(key));

            if (!value.HasValue) return null;

            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, Get(GetKeyForRedis(item))));

            return dict;
        }

        /// <summary>
        /// 获取缓存集合（异步方式）
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            throw new Exception("未实现");
        }

        #endregion

        #region 修改缓存

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public Task<bool> ReplaceAsync(string key, object value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            throw new Exception("未实现");
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresSliding, expiressAbsoulte);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            throw new Exception("未实现");
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (Exists(key))
                if (!Remove(key)) return false;

            return Add(key, value, expiresIn, isSliding);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            throw new Exception("未实现");
        }

        #endregion

        /// <summary>
        /// 组合Key值和实例名，就是Key值转为 实例名+Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKeyForRedis(string key)
        {
            return _instance + key;
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
