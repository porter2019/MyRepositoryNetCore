/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体分页查询所需实体参数                                                    
*│　作    者：杨习友                                            
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 15:32:02                           
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
        /// 创建时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string CreatedDate { get; set; }

    }
}