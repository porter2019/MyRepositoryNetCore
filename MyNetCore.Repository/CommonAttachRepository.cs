/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：附件表仓储实现                                                    
*│　作    者：杨习友                                             
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 22:32:57                            
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
    /// 附件表仓储实现
    /// </summary>
    public class CommonAttachRepository : BaseMyRepository<CommonAttach, int>, ICommonAttachRepository
    {
        private readonly IFreeSql _freeSql;

        public CommonAttachRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
