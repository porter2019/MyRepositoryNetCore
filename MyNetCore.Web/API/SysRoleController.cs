﻿using Microsoft.AspNetCore.Http;
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

namespace MyNetCore.Web.API
{
    /// <summary>
    /// 用户组管理
    /// </summary>
    [PermissionHandler("系统管理", "用户组", "sysRole", 20)]
    public class SysRoleController : BaseOpenAPIController
    {
        private readonly ILogger<SysRoleController> _logger;
        private readonly ISysRoleServices _sysRoleServices;

        public SysRoleController(ILogger<SysRoleController> logger, ISysRoleServices sysRoleServices)
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
        public async Task<ApiResult> GetPageList(Model.RequestViewModel.SysRolePageModel model)
        {
            var data = await _sysRoleServices.GetPageListAsync(model.PageOptions, out long total);

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
            var data = await _sysRoleServices.GetModelAsync(id);

            return ApiResult.OK(data);
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

    }
}
