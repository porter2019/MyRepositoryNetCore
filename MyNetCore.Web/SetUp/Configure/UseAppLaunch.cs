using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web.SetUp
{
    public static class UseAppLaunch
    {
        /// <summary>
        /// 程序启动/停止进行的操作
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMyAppLaunch(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var services = app.ApplicationServices.CreateScope().ServiceProvider;

            var logger = NLog.LogManager.GetCurrentClassLogger();

            var lifeTime = services.GetService<IHostApplicationLifetime>();

            var freeSql = services.GetService<IFreeSql>();


            lifeTime.ApplicationStarted.Register(() =>
            {
                logger.Info("网站启动..");

            });

            lifeTime.ApplicationStopping.Register(() =>
            {
                logger.Info("网站停止..");
            });

            return app;
        }
    }
}
