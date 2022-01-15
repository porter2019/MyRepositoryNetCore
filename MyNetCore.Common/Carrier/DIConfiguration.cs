using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyNetCore
{
    /// <summary>
    /// 从注入的服务中获取IConfiguraton对象
    /// </summary>
    public class DIConfiguration
    {
        protected readonly IConfiguration _config;

        public DIConfiguration()
        {
            if (ServiceLocator.Instance == null) throw new Exception("单例ServiceLocator.Instance为NULL");
            _config = ServiceLocator.Instance.GetService<IConfiguration>();
            if (_config == null) throw new Exception("获取不到注入的文件配置对象");
        }
    }
}