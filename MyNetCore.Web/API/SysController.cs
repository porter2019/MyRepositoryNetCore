using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web.API
{
    /// <summary>
    /// 系统接口
    /// </summary>
    [PermissionHandler("系统管理", "系统配置", "Sys", 10)]
    public class SysController : BaseOpenAPIController
    {
        private readonly ILogger<SysController> _logger;

        public SysController(ILogger<SysController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 测试接口是否已通
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test")]
        public ApiResult Test()
        {
            _logger.LogInformation("OK");
            return ApiResult.OK(AppSettings.Get<string>("DBContexts:SqlServer:ConnectionString"));//"DBContexts", "SqlServer", "LazyLoading"
        }



    }
}
