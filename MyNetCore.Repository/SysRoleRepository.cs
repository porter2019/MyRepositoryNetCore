/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统用户组仓储实现                                                    
*│　作    者：litdev                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2021-03-04 09:04:26                            
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
    /// 系统用户组仓储实现
    /// </summary>
    public class SysRoleRepository : BaseMyRepository<SysRole, int>, ISysRoleRepository
    {
        private readonly IFreeSql _freeSql;

        public SysRoleRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
