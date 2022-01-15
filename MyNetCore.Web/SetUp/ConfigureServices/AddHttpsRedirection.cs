namespace MyNetCore.Web.SetUp
{
    public static class AddHttpsRedirection
    {
        /// <summary>
        /// 强制跳转到https
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddHttpsRedirectionServices(this IServiceCollection services, IConfiguration config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (config.GetValue<bool>("Service:IsHttps"))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = config.GetValue<int>("Service:Port");
                });
            }
        }
    }
}