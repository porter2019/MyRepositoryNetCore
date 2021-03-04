using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyNetCore.IServices;
using MyNetCore.Services;

namespace MyNetCore.Web.SetUp
{
    public static class AddCustomServices
    {
        /// <summary>
        /// 注入业务类Services
        /// </summary>
        /// <param name="services"></param>
        public static void AddMyCustomServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //TODO 改为批量注入
            services.AddScoped<ISysUserServices, SysUserServices>();

        }

    }
}
