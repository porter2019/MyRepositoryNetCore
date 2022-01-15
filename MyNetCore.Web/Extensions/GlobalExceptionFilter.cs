using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

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

            if (context.Exception.GetType().Equals(typeof(CustomException)))
            {
                //只有指定了InnerException的才会记录，不然Seq里太多异常日志了，不方便排查关键性问题
                if (context.Exception.InnerException != null)
                {
                    _logger.LogError(context.Exception.InnerException, "发生异常(手动)");
                }
            }
            else
            {
                _logger.LogError(context.Exception, "发生异常");
            }

            //开发环境抛出全部异常
            //if (_hostEnvironment.IsDevelopment()) return;

            context.Result = new ObjectResult(ApiResult.Error(context.Exception.Message));

            context.ExceptionHandled = true;
        }
    }
}