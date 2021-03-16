using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.Model.CodeGenerate;

namespace MyNetCore.Services
{
    /// <summary>
    /// 代码生成服务
    /// </summary>
    [ServiceLifetime()]
    public class CodeGenerateServices : ICodeGenerateServices
    {
        private IViewRenderService _viewRender;

        public CodeGenerateServices(IViewRenderService viewRender)
        {
            _viewRender = viewRender;
        }

        /// <summary>
        /// 生成实体IRepository、IServices、Repository、Services四个层的代码文件
        /// </summary>
        /// <param name="modelName">实体名称，类名而非文件名，MyNetCore.Model.Entity下</param>
        /// <returns></returns>
        public async Task<ApiResult> GenerateIIRSCodeFile(string modelName)
        {
            var model = new BaseCode(modelName, GetModelDesc(modelName));
            //IRepository
            var html = await _viewRender.RenderViewToStringAsync("/Views/CodeGenerateTemplate/IRepositoryTemplate.cshtml", model);
            WriteCodeToFile(html, $"I{modelName}Repository.cs", "IRepository\\");
            //IServices
            html = await _viewRender.RenderViewToStringAsync("/Views/CodeGenerateTemplate/IServicesTemplate.cshtml", model);
            WriteCodeToFile(html, $"I{modelName}Services.cs", "IServices\\");
            //Repository
            html = await _viewRender.RenderViewToStringAsync("/Views/CodeGenerateTemplate/RepositoryTemplate.cshtml", model);
            WriteCodeToFile(html, $"{modelName}Repository.cs", "Repository\\");
            //Services
            html = await _viewRender.RenderViewToStringAsync("/Views/CodeGenerateTemplate/ServicesTemplate.cshtml", model);
            WriteCodeToFile(html, $"{modelName}Services.cs", "Services\\");

            return ApiResult.OK();
        }

        /// <summary>
        /// 保存文件到指定的目录
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <param name="projectLastName"></param>
        /// <returns></returns>
        private void WriteCodeToFile(string content, string fileName, string projectLastName)
        {
            throw new Exception("待完成");
        }

        /// <summary>
        /// 反射获取实体类的备注
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        private string GetModelDesc(string modelName)
        {
            var fristNameSpace = $"{modelName.GetProjectMainName()}.Model";
            var entityType = Assembly.Load(fristNameSpace).GetType($"{fristNameSpace}.Entity.{modelName}");
            if (entityType == null) throw new NullReferenceException($"找不到指定类:{fristNameSpace}.Entity.{modelName}");
            var fsTableInfo = entityType.GetCustomAttributes(typeof(Model.FsTableAttribute), true)[0] as Model.FsTableAttribute;
            if (fsTableInfo == null) throw new NullReferenceException($"指定类未指定【FsTable】标识");
            return fsTableInfo.DisplayName;
        }

    }
}
