/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：书籍信息分页查询所需实体参数                                                    
*│　作    者：杨习友                                            
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-06 20:19:48                           
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 查询书籍信息分页列表所需参数
    /// </summary>
    public class BookInfoPageModel : BaseRequestPageViewModel<Entity.BookInfoView>
    {
        /// <summary>
        /// 名称查询
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex)]
        public string Name { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.Equal)]
        public int? CategoryId { get; set; }

        /// <summary>
        /// 分类，包含子集查询
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.LikeLeft)]
        public string CategoryFullId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string CreatedDate { get; set; }

    }
}