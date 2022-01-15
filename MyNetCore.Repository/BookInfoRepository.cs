/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：书籍信息仓储实现
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-06 20:19:48
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Repository
{
    /// <summary>
    /// 书籍信息仓储实现
    /// </summary>
    public class BookInfoRepository : BaseMyRepository<BookInfo, int>, IBookInfoRepository
    {
        public BookInfoRepository(ILogger<BookInfoRepository> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {
        }
    }
}