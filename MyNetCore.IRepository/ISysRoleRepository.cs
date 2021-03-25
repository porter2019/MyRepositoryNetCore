/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统用户组仓储接口                                                    
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
using FreeSql;
using MyNetCore.Model.Entity;

namespace MyNetCore.IRepository
{
    /// <summary>
    /// 系统用户组仓储接口
    /// </summary>
    public interface ISysRoleRepository : IBaseMyRepository<SysRole>
    {
        /// <summary>
        /// 获取用户组的权限信息
        /// </summary>
        /// <param name="roleId">用户组id</param>
        /// <returns></returns>
        Task<List<Model.Dto.SysRoleModuleGroupModel>> GetPermitListByRoleId(int roleId);

        /// <summary>
        /// 设置用户组权限
        /// </summary>
        /// <param name="roleId">用户组id</param>
        /// <param name="permits">权限id组</param>
        /// <returns></returns>
        Task<bool> SetRolePermit(int roleId, string permits);

    }
}
