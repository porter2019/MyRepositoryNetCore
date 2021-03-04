using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace MyNetCore.Web.SetUp
{
    public static class UseWebMVC
    {
        /// <summary>
        /// 使用MVC组件
        /// </summary>
        /// <param name="app"></param>
        public static void UseMyWebMVC([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(new Common.Helper.NlogTraceListener());

            if (AppSettings.Get<bool>("Session", "IsEnabled"))
            {
                app.UseSession();
            }

            app.UseRouting();

            var isCORS = AppSettings.Get<bool>("CORS", "IsEnabled");
            if (isCORS) app.UseCors(GlobalVar.AllowSpecificOrigins);

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
