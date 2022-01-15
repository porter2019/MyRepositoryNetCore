/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体仓储实现
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-28 15:32:02
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Repository
{
    /// <summary>
    /// 演示主体仓储实现
    /// </summary>
    public class DemoMainRepository : BaseMyRepository<DemoMain, int>, IDemoMainRepository
    {
        public DemoMainRepository(ILogger<DemoMainRepository> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {
        }

        /// <summary>
        /// 根据外键id获取明细列表
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public Task<List<DemoMainItem>> GetDemoMainItems(int mainId)
        {
            return _fsql.Select<DemoMainItem>().Where(p => p.MainId == mainId).ToListAsync();
        }
    }
}