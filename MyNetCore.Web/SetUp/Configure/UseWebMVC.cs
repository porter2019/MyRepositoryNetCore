using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MyNetCore.Web.SetUp
{
    public static class UseWebMVC
    {
        /// <summary>
        /// 使用MVC组件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void UseMyWebMVC([NotNull] this IApplicationBuilder app, IConfiguration config)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(new Common.Helper.NlogTraceListener());

            app.UseSession();

            app.UseRouting();

            var isCORS = config.GetValue<bool>("CORS:IsEnabled");
            if (isCORS) app.UseCors(GlobalVar.AllowSpecificOrigins);

            //放到最后执行
            app.MapWhen(context =>
            {
                return context.Request.Headers["accept"].ToString().ToLower().StartsWith("image");
            },
            builder =>
            {
                builder.UseDefaultImage(defaultImagePath: System.IO.Path.Combine(config[GlobalVar.ConfigKeyPath_StaticFilesDirectoryKey], config["DefaultImagePath"]));
            });

            app.UseEndpoints(endpoints =>
            {
                if (isCORS)
                {
                    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireCors(GlobalVar.AllowSpecificOrigins);
                }
                else
                {
                    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                }
            });

        }
    }
}
