using Winton.Extensions.Configuration.Consul;

namespace MyNetCore.Web.SetUp
{
    /// <summary>
    /// 使用Consul配置文件
    /// </summary>
    public static class AddConsulKV
    {
        /// <summary>
        /// 使用ConsulKV配置文件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="env"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddMyConsulKV(this IServiceCollection services, ConfigurationManager config, IWebHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (!config.GetValue<bool>("ConsulKV:IsEnabled")) return;

            string consulServerUrl = config.GetValue<string>("ConsulKV:ServerUrl");
            string folderName = config.GetValue<string>("ConsulKV:Folder");
            if (folderName.IsNotNull()) folderName += "/";
            else folderName = "";

            config.AddConsul($"{folderName}public.{env.EnvironmentName}.json", options => //公共文件
            {
                options.Optional = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consulServerUrl); };
            });
            config.AddConsul($"{folderName}{env.ApplicationName}.{env.EnvironmentName}.json", options =>
            {
                options.Optional = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consulServerUrl); };
            });

        }
    }
}
