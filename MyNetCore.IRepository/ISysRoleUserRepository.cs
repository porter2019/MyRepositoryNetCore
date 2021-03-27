/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户组的用户仓储接口
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-27 13:03:08
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
    /// 用户组的用户仓储接口
    /// </summary>
    public interface ISysRoleUserRepository : IBaseMyRepository<SysRoleUser>
    {

    }
}
