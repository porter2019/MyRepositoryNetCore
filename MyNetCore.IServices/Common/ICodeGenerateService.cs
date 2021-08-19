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
    public interface ICodeGenerateService : IBatchDIServicesTag
    {
        /// <summary>
        /// 生成实体IRepository、IServices、Repository、Services四个层的代码文件
        /// </summary>
        /// <param name="modelName">实体名称，类名而非文件名，MyNetCore.Model.Entity下</param>
        /// <returns></returns>
        Task<ApiResult> GenerateIIRSCodeFile(string modelName);

        /// <summary>
        /// 生成Api控制器文件
        /// </summary>
        /// <param name="name">类名/文件名</param>
        /// <param name="desc">说明，如果为空，则表示name是Entity实体，该值自动反射从实体中取得</param>
        /// <returns></returns>
        Task<ApiResult> GenerateApiControllerFile(string name, string desc);

        /// <summary>
        /// 生成前端Vue页面代码
        /// </summary>
        /// <param name="name">类名/文件名</param>
        /// <param name="desc">说明，如果为空，则表示name是Entity实体，该值自动反射从实体中取得，否则生成基础前端模板</param>
        /// <returns></returns>
        Task<ApiResult> GenerateVuePageFile(string name, string desc);

    }
}
