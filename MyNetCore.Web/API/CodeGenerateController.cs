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

namespace MyNetCore.Web.API
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    public class CodeGenerateController : BaseOpenAPIController
    {
        public ILogger<CodeGenerateController> _logger;
        public ICodeGenerateServices _codeGenerateServices;

        public CodeGenerateController(ILogger<CodeGenerateController> logger, ICodeGenerateServices codeGenerateServices)
        {
            _logger = logger;
            _codeGenerateServices = codeGenerateServices;
        }

        ///// <summary>
        ///// 测试
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Route("test")]
        //public async Task<ContentResult> Test()
        //{
        //    var model = new Model.Entity.SysUser()
        //    {
        //        UserName = "ABC",
        //        LoginName = "EDFD"
        //    };
        //    var html = await _viewRender.RenderViewToStringAsync("/Views/CodeGenerateTemplate/Test.cshtml", model);
        //    return Content(html);
        //}

        /// <summary>
        /// 生成实体IRepository、IServices、Repository、Services四个层的代码文件
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("iirs")]
        public async Task<ApiResult> GenerateFourLayer(string modelName)
        {
            return await _codeGenerateServices.GenerateIIRSCodeFile(modelName);

        }


    }
}
