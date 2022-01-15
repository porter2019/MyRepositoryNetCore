using Microsoft.Extensions.Caching.Memory;

namespace MyNetCore.Services
{
    /// <summary>
    /// MemoryCache 实现
    /// </summary>
    [ServiceLifetime(true)]
    public class MemoryCacheService : ICacheService
    {
        protected IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
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
            return _cache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 验证缓存项是否存在（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return await Task.Run(() => _cache.TryGetValue(key, out _));
        }

        #endregion 是否存在

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
            _cache.Set(key, value);
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            return await Task.Run(() =>
            {
                _cache.Set(key, value);
                return Exists(key);
            });
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
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte));
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return await Task.Run(() =>
            {
                _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte));
                return Exists(key);
            });
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

            if (isSliding)
            {
                _cache.Set(key, value,
                            new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(expiresIn));
            }
            else
            {
                _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(expiresIn));
            }
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return await Task.Run(() =>
            {
                if (isSliding)
                {
                    _cache.Set(key, value,
                                new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(expiresIn));
                }
                else
                {
                    _cache.Set(key, value,
                        new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(expiresIn));
                }
                return Exists(key);
            });
        }

        #endregion 添加缓存

        #region 删除缓存

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);
            return !Exists(key);
        }

        /// <summary>
        /// 删除缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return await Task.Run(() =>
            {
                _cache.Remove(key);
                return !Exists(key);
            });
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => _cache.Remove(item));
        }

        /// <summary>
        /// 批量删除缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            await Task.Run(() => keys.ToList().ForEach(item => _cache.Remove(item)));
        }

        #endregion 删除缓存

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return _cache.Get<string>(key);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return await Task.Run(() => _cache.Get<string>(key));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return _cache.Get<T>(key);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return await Task.Run(() => _cache.Get<T>(key));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object GetObj(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return _cache.Get(key);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<object> GetObjAsync(string key)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));

            return await Task.Run(() => _cache.Get(key));
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
            keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));
            return dict;
        }

        /// <summary>
        /// 获取缓存集合（异步方式）
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            return await Task.Run(() =>
            {
                var dict = new Dictionary<string, object>();
                keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));
                return dict;
            });
        }

        #endregion 获取缓存

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
            {
                if (!Remove(key)) return false;
            }
            return Add(key, value);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (Exists(key))
            {
                if (!Remove(key)) return await Task.Run(() => false);
            }
            return await AddAsync(key, value);
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
            {
                if (!Remove(key)) return false;
            }
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
        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (Exists(key))
            {
                if (!Remove(key)) return await Task.Run(() => false);
            }
            return await AddAsync(key, value, expiresSliding, expiressAbsoulte);
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
            {
                if (!Remove(key)) return false;
            }
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
        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (Exists(key))
            {
                if (!Remove(key)) return await Task.Run(() => false);
            }
            return await AddAsync(key, value, expiresIn, isSliding);
        }

        #endregion 修改缓存

        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}