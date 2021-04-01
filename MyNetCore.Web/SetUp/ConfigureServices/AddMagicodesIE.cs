using System;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.DependencyInjection;

namespace MyNetCore.Web.SetUp
{
    /// <summary>
    /// Excel导入导出
    /// </summary>
    public static class AddMagicodesIE
    {
        /// <summary>
        /// 注入Magicodes.IE Excel导入导出
        /// </summary>
        /// <param name="services"></param>
        public static void AddMagicodesIEServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //导出
            services.AddScoped<IExporter, ExcelExporter>();
            //导入
            services.AddScoped<IImporter, ExcelImporter>();

        }
    }
}
