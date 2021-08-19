using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winton.Extensions.Configuration.Consul;

namespace MyNetCore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog($"nlog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.config").GetCurrentClassLogger();

            try
            {
                var webHost = CreateHostBuilder(args).Build();

                //�ȴ�ϵͳע����Ϻ�ִ��һЩ����
                //var config = (IConfiguration)webHost.Services.GetService(typeof(IConfiguration));
                //var dbDashboard = (IFreeSql<Tools.DashboardDBFlag>)webHost.Services.GetService(typeof(IFreeSql<Tools.DashboardDBFlag>));
                //Business.FreeSql.Dashboard.SettingsSysDAL settingsSysDAL = new Business.FreeSql.Dashboard.SettingsSysDAL(dbDashboard);
                //settingsSysDAL.Register(AppDomain.CurrentDomain.FriendlyName, config.GetSection("SysInfo:SysName").Value);

                webHost.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "���������쳣");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var env = hostingContext.HostingEnvironment;
                        hostingContext.Configuration = config.Build();
                        //ʹ��Consul��K/V�滻appsetting.json�����ļ�
                        if (hostingContext.Configuration.GetValue<bool>("ConsulKV:IsEnabled"))
                        {
                            string consulServerUrl = hostingContext.Configuration["ConsulKV:ServerUrl"];
                            string folderName = hostingContext.Configuration["ConsulKV:Folder"];
                            if (folderName.IsNotNull()) folderName += "/";
                            else folderName = "";
                            //ʹ��Consule�ͻ��˼�������
                            config.AddConsul($"{folderName}public.{env.EnvironmentName}.json", options => //�����ļ�
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
                            hostingContext.Configuration = config.Build();
                        }
                    });

                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logger =>
                {
                    logger.ClearProviders();
                    logger.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
