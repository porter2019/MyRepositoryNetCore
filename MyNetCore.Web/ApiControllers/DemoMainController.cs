#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-03-28 16:06:58
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 演示主体管理
    /// </summary>
	[PermissionHandler("演示", "演示主体", "demoMain", 10)]
    public class DemoMainController : BaseOpenApiController
    {
        private readonly ILogger<DemoMainController> _logger;
        private readonly IDemoMainService _demoMainService;

        public DemoMainController(ILogger<DemoMainController> logger, IDemoMainService demoMainService)
        {
            _logger = logger;
            _demoMainService = demoMainService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("get/pagelist")]
        [Permission("查看", "show")]
        public async Task<ApiResult> GetPageList(Model.RequestModel.DemoMainPageModel model)
        {
            var data = await _demoMainService.GetPageListBasicAsync(model, out long total);

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
            var data = await _demoMainService.GetModelFullAsync(id);

            return ApiResult.OK(data);
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modify")]
        [Permission("编辑", "modify")]
        public async Task<ApiResult> Post(Model.Entity.DemoMain model)
        {
            var data = await _demoMainService.ModifyAsync(model);

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
            var affrows = await _demoMainService.DeleteByIdsAsync(ids.SplitWithComma().ConvertIntArray());

            return ApiResult.OK($"受影响的行数:{affrows}");
        }
    }
}