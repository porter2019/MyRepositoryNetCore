/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体仓储接口
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-28 15:32:02
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.IRepository
{
    /// <summary>
    /// 演示主体仓储接口
    /// </summary>
    public interface IDemoMainRepository : IBaseMyRepository<DemoMain>
    {
        /// <summary>
        /// 根据外键id获取明细列表
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        Task<List<DemoMainItem>> GetDemoMainItems(int mainId);
    }
}