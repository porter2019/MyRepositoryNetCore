using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MyNetCore.Web.SetUp
{
    public static class AddHttpsRedirection
    {
        /// <summary>
        /// 强制跳转到https
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpsRedirectionServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.Get<bool>("Https", "Redirection"))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = AppSettings.Get<int>("Https", "Port");
                });
            }

        }
    }
}
