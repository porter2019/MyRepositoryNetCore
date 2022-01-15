/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：书籍信息业务逻辑接口
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-06 20:19:48
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.IServices
{
    /// <summary>
    /// 书籍信息业务类接口
    /// </summary>
    public interface IBookInfoService : IBaseService<BookInfo>
    {
        /// <summary>
        /// 获取完整的model信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BookInfoView> GetModelFullAsync(int id);

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<BookInfoView> ModifyAsync(BookInfoView entity);
    }
}