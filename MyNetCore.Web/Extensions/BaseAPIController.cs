using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web
{
    /// <summary>
    /// WebAPI基类,所有web api都必须直接或间接继承该类
    /// </summary>
    [ApiController, Produces("application/json")]
    public class BaseAPIController : ControllerBase
    {
        /// <summary>
        /// 通过此对象来获取指定注入的服务
        /// </summary>
        protected IHttpContextAccessor _hca;

        /// <summary>
        /// 环境信息
        /// </summary>
        protected readonly IHostEnvironment _hostEnvironment;

        public BaseAPIController()
        {
            _hca = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
            _hostEnvironment = _hca.HttpContext.RequestServices.GetService<IHostEnvironment>();
        }

    }
}
