using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace MyNetCore.Web.SetUp
{
    public static class SwaggerSetUp
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var projectName = AppDomain.CurrentDomain.FriendlyName;
            var projectMainName = projectName.Substring(0, projectName.LastIndexOf("."));

            var apiName = projectMainName;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = $"{apiName} 接口文档-NetCore 5.0",
                    Description = $"{apiName} Http API V1",
                });
                c.OrderActionsBy(o => o.RelativePath);

                var xmlList = new[] { $"{projectMainName}.Web.xml", $"{projectMainName}.Model.xml" };
                for (int i = 0; i < xmlList.Length; i++)
                {

                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlList[i]);
                    c.IncludeXmlComments(xmlPath, i == 0);
                }
            });
        }
    }
}
