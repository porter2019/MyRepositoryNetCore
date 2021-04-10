/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：Excel导入导出接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-01 20:17:04
*└──────────────────────────────────────────────────────────────┘
*/

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
using Magicodes.ExporterAndImporter.Core;
using System.IO;

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// Excel导入导出管理
    /// </summary>
	public class ExcelController : BaseOpenApiController
    {
        private readonly ILogger<ExcelController> _logger;
        private readonly IExporter _exporter;
        private readonly IImporter _importer;

        public ExcelController(ILogger<ExcelController> logger, IExporter exporter, IImporter importer)
        {
            _logger = logger;
            _exporter = exporter;
            _importer = importer;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("output/demo1")]
        public async Task<ApiResult> OutPutDemo1([FromServices] IDemoMainServices _demoMainServices)
        {
            var data = await _demoMainServices.GetListAsync(p => p.MainId > 0);
            //需要将List置为null
            data.ForEach(item =>
            {
                item.Attachs = null;
                item.ImageList = null;
                item.Items = null;
            });
            var excel = await _exporter.Export("output.xlsx", data);
            return ApiResult.OK(excel.FileName);
        }

        /// <summary>
        /// 生成导入的模板
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("output/template")]
        [Permission(Anonymous = true)]
        public async Task<FileContentResult> GenerateImportTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), nameof(Model.Entity.DemoMain) + ".xlsx");
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            var result = await _importer.GenerateTemplate<Model.Dto.ExcelImportDemo>(filePath);
            var fileByte = await System.IO.File.ReadAllBytesAsync(result.FileName);
            return File(fileByte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "测试导入模板.xlsx");

        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("import/test")]
        [Permission(Anonymous = true)]
        public ApiResult ImportData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "testImport.xlsx");
            var result = _importer.Import<Model.Dto.ExcelImportDemo>(filePath);
            if (result.Result.HasError)
            {
                return ApiResult.Error(result.Result.Exception.Message);
            }
            else
            {
                var data = result.Result.Data.ToList();

                return ApiResult.OK($"识别{data.Count}条数据");
            }

        }

    }
}
