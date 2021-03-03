/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统功能仓储实现                                                    
*│　作    者：litdev                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2021-03-03 17:17:45                            
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
    /// 系统功能仓储实现
    /// </summary>
    public class SysHandlerRepository : BaseMyRepository<SysHandler, int>, ISysHandlerRepository
    {
        private readonly IFreeSql _freeSql;

        public SysHandlerRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
