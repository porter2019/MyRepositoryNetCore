#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：计划任务示例接口控制器
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-11 11:44:18
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 计划任务示例
    /// </summary>
    [HiddenApi]
    public class TaskJobController : BaseOpenApiController
    {
        private readonly ILogger<TaskJobController> _logger;
        private ITaskJobService _taskJobServices;

        public TaskJobController(ILogger<TaskJobController> logger, ITaskJobService taskJobServices)
        {
            _logger = logger;
            _taskJobServices = taskJobServices;
        }

        /// <summary>
        /// 添加一个一次性任务，指定多少分钟后执行
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("add/backgroud/delay")]
        public Task<ApiResult> AddBackgroudJobDelay()
        {
            return _taskJobServices.AddBackgroudJobDelayAsync(new Model.AddTaskJobModel("测试访问百度BackgroundJobDelay", "https://www.baidu.com", "Get", 1));
        }

        /// <summary>
        /// 添加一个一次性任务，指定执行时间
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("add/backgroud/runat")]
        public Task<ApiResult> AddBackgroudJobRunAt()
        {
            var runAt = DateTime.Now.AddMinutes(2);
            return _taskJobServices.AddBackgroudJobRunAtAsync(new Model.AddTaskJobModel("测试访问百度BackgroudJobRunAt", "https://www.baidu.com", "Get", runAt));
        }

        /// <summary>
        /// 添加一个指定Corn的任务
        /// </summary>
        /// <param name="jobName">指定JobName，删除时也需要</param>
        /// <returns></returns>
        [HttpGet, Route("add/recurring")]
        public Task<ApiResult> AddRecurringCornJob(string jobName)
        {
            string corn = "";
            return _taskJobServices.AddRecurringCornJob(new Model.AddTaskJobModel(jobName, "https://www.baidu.com", "Get", corn));
        }

        /// <summary>
        /// 删除Background任务
        /// </summary>
        /// <param name="jobId">添加时返回的jobId</param>
        /// <returns></returns>
        [HttpGet, Route("delete/background")]
        public Task<ApiResult> DeleteBackgroudJob(string jobId)
        {
            return _taskJobServices.RemoveBackgroundJobAsync(jobId);
        }

        /// <summary>
        /// 删除Recurring任务
        /// </summary>
        /// <param name="jobName">添加时指定的JobName</param>
        /// <returns></returns>
        [HttpGet, Route("delete/recurring")]
        public Task<ApiResult> DeleteRecurringJob(string jobName)
        {
            return _taskJobServices.RemoveRecurringJobAsync(jobName);
        }
    }
}