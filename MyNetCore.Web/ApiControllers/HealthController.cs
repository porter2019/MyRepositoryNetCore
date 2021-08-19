using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
