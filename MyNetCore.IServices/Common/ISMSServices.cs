using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{

    /// <summary>
    /// SMS短信验证码服务
    /// </summary>
    public interface ISMSServices : IBatchDIServicesTag
    {

        /// <summary>
        /// 发送测试验证码
        /// </summary>
        /// <param name="guid">唯一标识</param>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ApiResult> SendTestAsync(string guid, string mobile, string code);

        /// <summary>
        /// 校验验证码是否有效
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ApiResult> ValidateCodeAsync(string guid, string code);

    }
}
