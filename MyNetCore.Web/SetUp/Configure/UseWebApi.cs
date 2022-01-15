using System.Diagnostics.CodeAnalysis;

namespace MyNetCore.Web.SetUp
{
    public static class UseWebApi
    {
        /// <summary>
        /// 使用Web Api组件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void UseMyWebApi([NotNull] this IApplicationBuilder app, IConfiguration config)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

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
                    endpoints.MapControllers().RequireCors(GlobalVar.AllowSpecificOrigins);
                else
                    endpoints.MapControllers();
            });
        }
    }
}