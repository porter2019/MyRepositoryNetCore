#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：书籍信息接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-06 20:19:55
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
    /// 书籍信息管理
    /// </summary>
	[PermissionHandler("演示", "书籍信息", "bookInfo", 10)]
	public class BookInfoController : BaseOpenApiController
    {
		private readonly ILogger<BookInfoController> _logger;
		private readonly IBookInfoService _bookInfoServices;
		
		public BookInfoController(ILogger<BookInfoController> logger, IBookInfoService bookInfoServices)
        {
            _logger = logger;
			_bookInfoServices = bookInfoServices;
        }
		
		/// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("get/pagelist")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetPageList(Model.RequestModel.BookInfoPageModel model)
        {
            var data = await _bookInfoServices.GetPageListViewBasicAsync(model, out long total);

            return ApiResult.OK(total, data);
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
            var data = await _bookInfoServices.GetModelFullAsync(id);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modify")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> Post(Model.Entity.BookInfoView model)
        {
            var data = await _bookInfoServices.ModifyAsync(model);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 根据ids批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete")]
        [Permission("删除", "delete")]
        public async Task<ApiResult> Delete(string ids)
        {
            var affrows = await _bookInfoServices.DeleteByIdsAsync(ids.SplitWithComma());

            return ApiResult.OK($"受影响的行数:{affrows}");
        }
		
    }
}
