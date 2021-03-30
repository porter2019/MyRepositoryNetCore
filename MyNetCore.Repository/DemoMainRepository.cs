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

        /// <summary>
        /// 根据外键id获取明细列表
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public Task<List<DemoMainItem>> GetDemoMainItems(int mainId)
        {
            return _freeSql.Select<DemoMainItem>().Where(p => p.MainId == mainId).ToListAsync();
        }

    }
}
