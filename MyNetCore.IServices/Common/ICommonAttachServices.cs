/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：附件表业务逻辑接口                                                    
*│　作    者：杨习友                                            
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 22:32:57                           
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 附件表业务类接口
    /// </summary>
    public interface ICommonAttachServices : IBaseServices<CommonAttach>
    {
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="refType"></param>
        /// <param name="field">关联字段</param>
        /// <returns></returns>
        Task<List<CommonAttach>> GetAttachListAsync(int refId, Type refType, string field = "Attach");
    }
}