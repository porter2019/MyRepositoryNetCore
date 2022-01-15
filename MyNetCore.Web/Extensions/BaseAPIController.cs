using Microsoft.Extensions.Hosting;

namespace MyNetCore.Web
{
    /// <summary>
    /// WebAPI基类,所有web api都必须直接或间接继承该类
    /// </summary>
    [ApiController, Produces("application/json")]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 通过此对象来获取指定注入的服务
        /// </summary>
        protected IHttpContextAccessor _hca;

        /// <summary>
        /// 环境信息
        /// </summary>
        protected readonly IHostEnvironment _hostEnvironment;

        //规范！不能在控制器里直接拿数据库上下文了，所有业务操作都要使用Services业务实现层
        //protected readonly IFreeSql _fsql;

        public BaseApiController()
        {
            _hca = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
            _hostEnvironment = _hca.HttpContext.RequestServices.GetService<IHostEnvironment>();
            //_fsql = _hca.HttpContext.RequestServices.GetService<IFreeSql>();
        }
    }
}