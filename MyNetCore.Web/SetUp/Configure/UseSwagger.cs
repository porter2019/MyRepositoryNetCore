using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MyNetCore.Web.SetUp
{
    public static class UseSwagger
    {
        /// <summary>
        /// 使用Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void UseMySwagger([NotNull] this IApplicationBuilder app, IConfiguration config)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (!config.GetValue<bool>("Swagger:IsEnabled")) return;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", "接口文档 V1");
                c.RoutePrefix = config["Swagger:RoutePrefix"];
            });
        }
    }
}
