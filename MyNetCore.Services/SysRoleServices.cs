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

namespace MyNetCore.Services
{
    /// <summary>
    /// 用户组业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class SysRoleServices : BaseServices<SysRole, int>, ISysRoleServices
    {
        private readonly ISysRoleRepository _sysRoleRepository;

        public SysRoleServices(SysRoleRepository sysRoleRepository) : base(sysRoleRepository)
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

    }
}
