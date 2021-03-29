/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体仓储实现                                                    
*│　作    者：杨习友                                             
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 15:32:02                            
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
    /// 演示主体仓储实现
    /// </summary>
    public class DemoMainRepository : BaseMyRepository<DemoMain, int>, IDemoMainRepository
    {
        private readonly IFreeSql _freeSql;

        public DemoMainRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
