﻿using Microsoft.Extensions.Hosting;

namespace MyNetCore.Web.SetUp
{
    public static class UseShowAllServices
    {
        /// <summary>
        /// 显示所有注册的服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAllServicesRoute(this IApplicationBuilder app, IWebHostEnvironment env, IServiceCollection services)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (!env.IsProduction())
            {
                app.Map("/allservices", builder => builder.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($"<h1>所有服务{services.Count}个</h1><table border=\"1\"><thead><tr><th>类型</th><th>生命周期</th><th>实例</th></tr></thead><tbody>");
                    foreach (var svc in services)
                    {
                        await context.Response.WriteAsync("<tr>");
                        await context.Response.WriteAsync($"<td>{svc.ServiceType.FullName}</td>");
                        await context.Response.WriteAsync($"<td>{svc.Lifetime}</td>");
                        await context.Response.WriteAsync($"<td>{svc.ImplementationType?.FullName}</td>");
                        await context.Response.WriteAsync("</tr>");
                    }
                    await context.Response.WriteAsync("</tbody></table>");
                }));
            }

            return app;
        }
    }
}