using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 代码生成服务
    /// </summary>
    public interface ICodeGenerateServices : IBatchDIServicesTag
    {
        /// <summary>
        /// 生成实体IRepository、IServices、Repository、Services四个层的代码文件
        /// </summary>
        /// <param name="modelName">实体名称，类名而非文件名，MyNetCore.Model.Entity下</param>
        /// <returns></returns>
        Task<ApiResult> GenerateIIRSCodeFile(string modelName);
    }
}
