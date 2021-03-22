using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MyNetCore.Web
{
    /// <summary>
    /// 全局异常捕捉
    /// </summary>
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IHostEnvironment hostEnvironment, ILogger<GlobalExceptionFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            //已处理
            if (context.ExceptionHandled) return;

            _logger.LogError(context.Exception, "发生异常");

            //开发环境抛出全部异常
            //if (_hostEnvironment.IsDevelopment()) return;

            var contentType = context.HttpContext.Request.Headers["Content-Type"].FirstOrDefault()?.ToLower() ?? "";
            var accept = context.HttpContext.Request.Headers["accept"].FirstOrDefault()?.ToLower() ?? "";
            if (contentType.Equals("application/json") || accept.Equals("application/json"))
            {
                context.Result = new ObjectResult(ApiResult.Error(context.Exception.Message));
            }
            else
            {
                context.Result = new ObjectResult("服务器出现异常");
            }

            context.ExceptionHandled = true;
        }
    }
}
