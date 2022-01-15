using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics.CodeAnalysis;

namespace MyNetCore.Web.SetUp
{
    public static class AddWebApi
    {
        /// <summary>
        /// 添加WebApi
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddWebApiServices([NotNull] this IServiceCollection services, IConfiguration config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //跨域
            if (config.GetValue<bool>("CORS:IsEnabled"))
            {
                var origins = config["CORS:AllowOrigins"];
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

            services.AddControllers(options =>
                    {
                        options.Filters.Add(typeof(GlobalExceptionFilter));
                    }).AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                        options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConfig.DateTimeConverter());
                        options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConfig.DateTimeNullableConverter());
                        options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConfig.IntToStringConverter());
                        options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConfig.DoubleToStringConverter());
                        options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConfig.DecimalToStringConverter());
                        options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConfig.StringJsonConverter());
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
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = int.MaxValue;
            //});
            //解除Form表单内容大小限制
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
            });
        }
    }
}