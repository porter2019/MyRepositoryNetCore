#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体分页查询所需实体参数                                                    
*│　作    者：杨习友                                            
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 15:32:02                           
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 查询演示主体分页列表所需参数
    /// </summary>
    public class DemoMainPageModel : BaseRequestPageViewModel<Entity.DemoMain>
    {

        /// <summary>
        /// 标题
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex)]
        public string Title { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string UpdatedDate { get; set; }

    }
}