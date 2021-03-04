using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace MyNetCore.Web.SetUp
{
    public static class UseSwagger
    {
        /// <summary>
        /// 使用Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseMySwagger([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (!AppSettings.Get<bool>("Swagger", "IsEnabled")) return;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", "接口文档 V1");
                c.RoutePrefix = AppSettings.Get("Swagger", "RoutePrefix");
            });
        }
    }
}
