/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：经典父子分类接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-04 17:31:33
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

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 经典父子分类管理
    /// </summary>
	[PermissionHandler("演示", "经典父子分类", "bookCategory", 10)]
    public class BookCategoryController : BaseOpenApiController
    {
        private readonly ILogger<BookCategoryController> _logger;
        private readonly IBookCategoryServices _bookCategoryServices;

        public BookCategoryController(ILogger<BookCategoryController> logger, IBookCategoryServices bookCategoryServices)
        {
            _logger = logger;
            _bookCategoryServices = bookCategoryServices;
        }

        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet, Route("get/list/tree")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetTreeList(string title)
        {
            var data = await _bookCategoryServices.GetTreeList(title);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 获取某条数据的父级或子级ids
        /// </summary>
        /// <param name="id">数据id</param>
        /// <param name="isParent">是否查找父级</param>
        /// <returns></returns>
        [HttpGet, Route("get/layer/ids")]
        public async Task<ApiResult> GetParentOrChildIds(int id, bool isParent)
        {
            var data = await _bookCategoryServices.GetParentOrChildIds(id, isParent);

            return ApiResult.OK("OK", data);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("get/info")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetInfo(int id)
        {
            if (id < 1) return ApiResult.OK(new Model.Entity.BookCategory());

            var data = await _bookCategoryServices.GetModelAsync(id);

            if (data == null) data = new Model.Entity.BookCategory();

            if (data.ParentId.ObjToInt(-1) > 0)
            {
                data.Parent = await _bookCategoryServices.GetModelAsync(data.ParentId.ObjToInt());
            }

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modify")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> Post(Model.Entity.BookCategory model)
        {
            var data = await _bookCategoryServices.InsertOrUpdateAsync(model);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 根据ids批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete")]
        [Permission("删除", "delete")]
        public async Task<ApiResult> Delete(int id)
        {
            var affrows = await _bookCategoryServices.DeleteIncludeChilds(id);

            return ApiResult.OK($"受影响的行数:{affrows}");
        }

    }
}
