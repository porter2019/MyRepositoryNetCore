/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户组的用户仓储实现                                                    
*│　作    者：杨习友                                             
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-27 13:03:08                            
*└──────────────────────────────────────────────────────────────┘
*/

using FreeSql;
using MyNetCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 用户组的用户仓储实现
    /// </summary>
    public class SysRoleUserRepository : BaseMyRepository<SysRoleUser, int>, ISysRoleUserRepository
    {
        private readonly IFreeSql _freeSql;

        public SysRoleUserRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
