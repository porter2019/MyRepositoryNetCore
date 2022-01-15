using Microsoft.OpenApi.Models;
using System.IO;

namespace MyNetCore.Web.SetUp
{
    public static class AddSwagger
    {
        /// <summary>
        /// 注入Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddSwaggerServices(this IServiceCollection services, IConfiguration config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (!config.GetValue<bool>("Swagger:IsEnabled")) return;

            var projectMainName = AppDomain.CurrentDomain.GetProjectMainName();

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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Token值",
                    Name = config[GlobalVar.ConfigKeyPath_AuthenticationTokenKey],
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });

                var xmlList = new[] { $"{projectMainName}.Web.xml", $"{projectMainName}.Model.xml" };
                for (int i = 0; i < xmlList.Length; i++)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlList[i]);
                    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, i == 0);
                }
                c.DocumentFilter<HiddenApiFilter>();
                c.SchemaFilter<HiddenApiSchemaFilter>();
            });
        }
    }
}