#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：经典父子分类分页查询所需实体参数
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-04 17:30:04
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 查询经典父子分类分页列表所需参数
    /// </summary>
    public class BookCategoryPageModel : BaseRequestPageViewModel<Entity.BookCategory>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex)]
        public string Title { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string CreatedDate { get; set; }
    }
}