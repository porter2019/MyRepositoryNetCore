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
    /// 用户组管理
    /// </summary>
    [PermissionHandler("系统管理", "用户组", "sysRole", 20)]
    public class SysRoleController : BaseOpenApiController
    {
        private readonly ILogger<SysRoleController> _logger;
        private readonly ISysRoleService _sysRoleServices;

        public SysRoleController(ILogger<SysRoleController> logger, ISysRoleService sysRoleServices)
        {
            _logger = logger;
            _sysRoleServices = sysRoleServices;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("get/pagelist")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetPageList(Model.RequestModel.SysRolePageModel model)
        {
            var data = await _sysRoleServices.GetPageListBasicAsync(model, out long total);

            return ApiResult.OK(total, data);
        }

        /// <summary>
        /// 获取所有的组列表（无需权限）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("get/list")]
        public async Task<ApiResult> GetAllList()
        {
            var data = await _sysRoleServices.GetListAsync(p => p.Status);

            return ApiResult.OK(data);
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
            if (id < 1)
            {
                return ApiResult.OK(new Model.Entity.SysRole());
            }

            var data = await _sysRoleServices.GetModelAsync(id);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 判断组名是否存在
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName">组名</param>
        /// <returns></returns>
        [HttpGet, Route("exists/rolename")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> CheckLoginNameExists(int roleId, string roleName)
        {
            var exists = await _sysRoleServices.CheckRoleNameExists(roleId, roleName);
            return ApiResult.OK(exists);
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modify")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> Post(Model.Entity.SysRole model)
        {
            var data = await _sysRoleServices.InsertOrUpdateAsync(model);

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
            var affrows = await _sysRoleServices.DeleteByIdsAsync(ids.SplitWithComma());

            return ApiResult.OK($"受影响的行数:{affrows}");
        }

        /// <summary>
        /// 获取用户组的权限配置列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet, Route("get/permit/list")]
        public async Task<ApiResult> GetRolePermitList(int roleId)
        {
            var data = await _sysRoleServices.GetPermitListByRoleId(roleId);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 更新用户组的权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permits"></param>
        /// <returns></returns>
        [HttpPost, Route("modify/permit")]
        public async Task<ApiResult> GetRolePermitList(int roleId, string permits)
        {
            var data = await _sysRoleServices.SetRolePermit(roleId, permits);

            return ApiResult.OK(data);
        }

    }
}
