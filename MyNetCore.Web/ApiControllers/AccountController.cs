﻿#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：账号相关接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-18 20:38:03
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 账号相关管理
    /// </summary>
	public class AccountController : BaseOpenApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ISysUserService _sysUserServices;

        public AccountController(ILogger<AccountController> logger, ISysUserService sysUserServices)
        {
            _logger = logger;
            _sysUserServices = sysUserServices;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("login"), Permission("登录", Anonymous = true)]
        public async Task<ApiResult> SignIn(Model.RequestModel.SysUserLoginModel model)
        {
            try
            {
                var data = await _sysUserServices.UserLogin(model);
                return ApiResult.OK(data);
            }
            catch (Exception ex)
            {
                return ApiResult.Failed(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户的权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("permissions")]
        [Permission("")]
        public async Task<ApiResult> GetPermissions()
        {
            var data = await _sysUserServices.GetPermissionsByUserId(CurrentUserInfo.UserId);
            return ApiResult.OK(data);
        }
    }
}