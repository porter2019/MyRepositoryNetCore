namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 站点健康检查
    /// </summary>
    [Route("api/[controller]"), HiddenApi]
    public class HealthController : BaseApiController
    {
        /// <summary>
        /// main
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("check")]
        public ApiResult Check()
        {
            return ApiResult.OK();
        }
    }
}