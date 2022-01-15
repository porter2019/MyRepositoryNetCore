/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：验证码发送记录表仓储实现
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-07 20:44:39
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Repository
{
    /// <summary>
    /// 验证码发送记录表仓储实现
    /// </summary>
    public class ValidateCodeHistoryRepository : BaseMyRepository<ValidateCodeHistory, int>, IValidateCodeHistoryRepository
    {
        public ValidateCodeHistoryRepository(ILogger<ValidateCodeHistory> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {
        }
    }
}