using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web.SetUp
{
    /// <summary>
    /// 使用Consul监控站点
    /// </summary>
    public static class UseConsul
    {
        /// <summary>
        /// 使用Consul监控站点
        /// </summary>
        /// <param name="app"></param>
        /// <param name="lifetime"></param>
        /// <param name="config"></param>
        public static void UseMyConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration config)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (!config.GetValue<bool>("Service:AddConsulWatch")) return;

            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{config["Consul:IP"]}:{config["Consul:Port"]}"));
            //健康检查
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(config.GetValue<int>("Service:HealthInterval")),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http{ (config.GetValue<bool>("Service:IsHttps") ? "s" : "") }://{config["Service:IP"]}:{config["Service:Port"]}{config["Service:HealthCheckPath"]}",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };
            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = config["Service:SubId"],
                Name = config["Service:Name"],
                Address = config["Service:IP"],
                Port = config.GetValue<int>("Service:Port"),
                Tags = config["Service:Tags"].SplitWithComma()//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                //服务停止时取消注册
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });


        }
    }
}
