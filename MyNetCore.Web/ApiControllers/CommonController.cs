using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController : BaseOpenApiController
    {
        private readonly ILogger<CommonController> _logger;

        public CommonController(ILogger<CommonController> logger)
        {
            _logger = logger;
        }

        #region 文件上传

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("upload")]
        public ApiResult FileUpload()
        {
            var saveResult = HttpContext.Request.Form.Files.SaveAttach(HttpContext.Request.Form["tag"].ToString());
            return ApiResult.OK(saveResult);
        }

        #endregion

    }
}
