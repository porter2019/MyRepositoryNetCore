namespace MyNetCore.Web.SetUp
{
    /// <summary>
    /// 请求出错处理
    /// </summary>
    public class WebResponseStatusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebResponseStatusMiddleware> _logger;

        public WebResponseStatusMiddleware(RequestDelegate next, ILogger<WebResponseStatusMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            var contentType = context.Request.Headers["Content-Type"].FirstOrDefault()?.ToLower() ?? "";
            if (contentType.Equals("application/json"))
            {
                if (context.Response.StatusCode == 404)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync(JsonHelper.Serialize(ApiResult.NotFound()), System.Text.Encoding.UTF8);
                }
                else if (context.Response.StatusCode == 401)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync(JsonHelper.Serialize(ApiResult.Anonymous()), System.Text.Encoding.UTF8);
                }
            }
            else
            {
                if (context.Response.StatusCode == 404)
                {
                    await context.Response.WriteAsync("404 Not Found", System.Text.Encoding.UTF8);
                }
            }
        }
    }

    public static class WebResponseStatusMiddlewareExtensions
    {
        /// <summary>
        /// 把404、401响应码改为200，并自定义输出
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebResponseStatus(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WebResponseStatusMiddleware>();
        }
    }
}