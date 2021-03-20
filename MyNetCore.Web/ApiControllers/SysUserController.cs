/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统用户接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-19 20:03:38
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
    /// 系统用户管理
    /// </summary>
	[PermissionHandler("所属模块", "系统用户", "sysUser", 10)]
    public class SysUserController : BaseOpenApiController
    {
        private readonly ILogger<SysUserController> _logger;
        private readonly ISysUserServices _sysUserServices;

        public SysUserController(ILogger<SysUserController> logger, ISysUserServices sysUserServices)
        {
            _logger = logger;
            _sysUserServices = sysUserServices;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("get/pagelist")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetPageList(Model.RequestModel.SysUserPageModel model)
        {
            var data = await _sysUserServices.GetPageListAsync(model, out long total);

            return ApiResult.OK(total, data);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("get/info")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetInfo(int id)
        {
            var data = await _sysUserServices.GetModelAsync(id);
            if (data == null) data = new Model.Entity.SysUser();
            else data.Password = "@@**@@";
            return ApiResult.OK(data);
        }

        /// <summary>
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        [HttpGet, Route("exists/loginName")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> CheckLoginNameExists(int userId, string loginName)
        {
            var exists = await _sysUserServices.CheckLoginNameExists(userId, loginName);
            return ApiResult.OK(exists);
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modify")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> Post(Model.Entity.SysUser model)
        {
            var data = await _sysUserServices.Modify(model);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 根据ids批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete")]
        [Permission("删除", "delete")]
        public async Task<ApiResult> Delete(string ids)
        {
            var affrows = await _sysUserServices.DeleteByIdsAsync(ids.SplitWithComma());

            return ApiResult.OK($"受影响的行数:{affrows}");
        }

    }
}
