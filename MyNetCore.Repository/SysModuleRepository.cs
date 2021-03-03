/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统模块仓储实现                                                    
*│　作    者：litdev                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2021-03-03 17:10:42                            
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
    /// 系统模块仓储实现
    /// </summary>
    public class SysModuleRepository : BaseMyRepository<SysModule, int>, ISysModuleRepository
    {
        private readonly IFreeSql _freeSql;

        public SysModuleRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
