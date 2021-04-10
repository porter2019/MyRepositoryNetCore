using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyNetCore.IServices;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    public class CodeGenerateController : BaseOpenApiController
    {
        public ILogger<CodeGenerateController> _logger;
        public ICodeGenerateServices _codeGenerateServices;

        public CodeGenerateController(ILogger<CodeGenerateController> logger, ICodeGenerateServices codeGenerateServices)
        {
            _logger = logger;
            _codeGenerateServices = codeGenerateServices;
        }

        /// <summary>
        /// 生成实体IRepository、IServices、Repository、Services四个层的代码文件
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("iirs")]
        public async Task<ApiResult> GenerateFourLayer(string modelName)
        {
            return await _codeGenerateServices.GenerateIIRSCodeFile(modelName);

        }

        /// <summary>
        /// 生成Api控制器文件
        /// </summary>
        /// <param name="name">类名/文件名</param>
        /// <param name="desc">说明，如果为空，则表示name是Entity实体，该值自动反射从实体中取得</param>
        /// <returns></returns>
        [HttpGet, Route("controller/api")]
        public async Task<ApiResult> GenerateApiController(string name, string desc)
        {
            return await _codeGenerateServices.GenerateApiControllerFile(name, desc);

        }

        /// <summary>
        /// 生成前端Vue页面代码，包括api、route、pages
        /// </summary>
        /// <param name="name">类名/文件名</param>
        /// <param name="desc">说明，如果为空，则表示name是Entity实体，该值自动反射从实体中取得</param>
        /// <returns></returns>
        [HttpGet, Route("vue/all")]
        public Task<ApiResult> GenerateVueFiles(string name, string desc)
        {
            return _codeGenerateServices.GenerateVuePageFile(name, desc);


        }

    }
}
