/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：账号相关接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-18 20:38:03
*└──────────────────────────────────────────────────────────────┘
*/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyNetCore.IServices;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 账号相关管理
    /// </summary>
	public class AccountController : BaseOpenApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ISysUserServices _sysUserServices;

        public AccountController(ILogger<AccountController> logger, ISysUserServices sysUserServices)
        {
            _logger = logger;
            _sysUserServices = sysUserServices;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("login"), Permission("登录", Anonymous = true)]
        public Task<ApiResult> SignIn(Model.RequestModel.SysUserLoginModel model)
        {
            return _sysUserServices.UserLogin(model);
        }

        /// <summary>
        /// 获取用户的权限列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("permissions")]
        [Permission("")]
        public Task<ApiResult> GetPermissions(Model.BaseRequestViewModel model)
        {
            return Task.FromResult(ApiResult.OK("OK", new List<string>() { "permission.browse", "permission.create", "permission.edit", "permission.remove", "sysRole.show", "sysRole.index" }));
        }

    }
}
