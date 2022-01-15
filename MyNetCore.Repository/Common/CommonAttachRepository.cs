/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：附件表仓储实现
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-28 22:32:57
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Repository
{
    /// <summary>
    /// 附件表仓储实现
    /// </summary>
    public class CommonAttachRepository : BaseMyRepository<CommonAttach, int>, ICommonAttachRepository
    {
        public CommonAttachRepository(ILogger<CommonAttachRepository> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {
        }
    }
}