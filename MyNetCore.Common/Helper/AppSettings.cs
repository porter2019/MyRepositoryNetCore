using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MyNetCore
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class AppSettings
    {
        private static IConfiguration _configuration;

        static AppSettings()
        {
            BuildConfiguration();
        }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_configuration.GetCurrentEnvironmentName()}.json", true, true);
            _configuration = builder.Build();
        }

        /// <summary>
        /// 读取指定节点信息
        /// </summary>
        /// <param name="key">节点名称，多节点以:分隔</param>
        public static string Get(string key)
        {
            if (key.IsNull()) return "";
            return _configuration[key];
        }

        /// <summary>
        /// 读取指定节点信息，按层级传入多个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(params string[] key)
        {
            if (key.Length == 0) return "";
            return _configuration[string.Join(':', key)];
        }

        /// <summary>
        /// 读取指定节点信息
        /// <code>此方法依赖Microsoft.Extensions.Configuration.Binder包</code>
        /// </summary>
        public static T Get<T>(string key)
        {
            System.Diagnostics.Trace.Write("Nlog：：："+key);
            try
            {
                return _configuration.GetValue<T>(key);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 读取指定节点信息，按层级传入多个key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <code>此方法依赖Microsoft.Extensions.Configuration.Binder包</code>
        /// <returns></returns>
        public static T Get<T>(params string[] key)
        {
            try
            {
                return _configuration.GetValue<T>(string.Join(':', key));
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
