/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：验证码发送记录表分页查询所需实体参数                                                    
*│　作    者：杨习友                                            
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-07 20:44:39                           
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
    /// 查询验证码发送记录表分页列表所需参数
    /// </summary>
    public class ValidateCodeHistoryPageModel : BaseRequestPageViewModel<Entity.ValidateCodeHistory>
    {
	    
		/// <summary>
        /// 创建时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string CreatedDate { get; set; }
		
    }
}