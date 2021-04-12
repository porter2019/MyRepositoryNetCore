/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：验证码发送记录表业务逻辑接口                                                    
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
using MyNetCore.Model.Entity;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 验证码发送记录表业务类接口
    /// </summary>
    public interface IValidateCodeHistoryServices : IBaseServices<ValidateCodeHistory>
    {

    }
}