using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace MyNetCore.Web.SetUp
{
    public static class AddWebMVC
    {
        /// <summary>
        /// 添加MVC
        /// </summary>
        /// <param name="services"></param>
        public static void AddWebMVCServices([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.Get<bool>("Session", "IsEnabled"))
            {
                services.AddSession();
            }

            //跨域
            if (AppSettings.Get<bool>("CORS", "IsEnabled"))
            {
                var origins = AppSettings.Get("CORS", "AllowOrigins");
                services.AddCors(option =>
                {
                    option.AddPolicy(GlobalVar.AllowSpecificOrigins, builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod();
                        if (origins.Trim().Equals("*")) builder.AllowAnyOrigin();
                        else builder.WithOrigins(origins.SplitWithSemicolon());
                    });
                });
            }

            //使用小写的URL
            services.AddRouting(option => option.LowercaseUrls = true);

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();//变量首字母不转小写
                    });

            //处理DTO验证错误时的返回消息格式
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => e.Value.Errors.First().ErrorMessage).ToArray();
                    return new ObjectResult(ApiResult.ValidateFail(string.Join(",", errors)));
                };
            });

            //解除IIS文件上传大小限制
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            //解除Form表单内容大小限制
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
            });

        }
    }
}
