/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统用户组业务逻辑接口                                                    
*│　作    者：litdev                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2021-03-04 09:04:26                            
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using MyNetCore.Model.Entity;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Services
{
    /// <summary>
    /// 用户组业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class SysRoleService : BaseService<SysRole, int>, ISysRoleService
    {
        private readonly ISysRoleRepository _sysRoleRepository;

        public SysRoleService(ILogger<SysRoleService> logger,
            ISysRoleRepository sysRoleRepository) : base(sysRoleRepository, logger)
        {
            _sysRoleRepository = sysRoleRepository;
        }

        /// <summary>
        /// 判断登录组名是否存在
        /// </summary>
        /// <param name="roleId">组id</param>
        /// <param name="roleName">组名</param>
        /// <returns></returns>
        public Task<bool> CheckRoleNameExists(int roleId, string roleName)
        {
            if (roleId > 0)
            {
                if (_sysRoleRepository.Exists(p => p.RoleId != roleId && p.RoleName == roleName))
                    return Task.FromResult(true);
            }
            else
            {
                if (_sysRoleRepository.Exists(p => p.RoleName == roleName))
                    return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// 获取用户组的权限信息
        /// </summary>
        /// <param name="roleId">用户组id</param>
        /// <returns></returns>
        public async Task<List<Model.Dto.SysRoleModuleGroupModel>> GetPermitListByRoleId(int roleId)
        {
            var roleEntity = await _sysRoleRepository.GetModelAsync(p => p.RoleId == roleId);
            if (roleEntity == null) throw new NullReferenceException("用户组不存在");
            if (roleEntity.IsSuper) throw new Exception("超级管理组无需配置权限");

            return await _sysRoleRepository.GetPermitListByRoleId(roleId);
        }

        /// <summary>
        /// 设置用户组权限
        /// </summary>
        /// <param name="roleId">组id</param>
        /// <param name="permits">权限id组</param>
        /// <returns></returns>
        public async Task<bool> SetRolePermit(int roleId, string permits)
        {
            var roleEntity = await _sysRoleRepository.GetModelAsync(p => p.RoleId == roleId);
            if (roleEntity == null) throw new NullReferenceException("用户组不存在");
            if (roleEntity.IsSuper) throw new Exception("超级管理组无需配置权限");

            return await _sysRoleRepository.SetRolePermit(roleId, permits);
        }

    }
}
