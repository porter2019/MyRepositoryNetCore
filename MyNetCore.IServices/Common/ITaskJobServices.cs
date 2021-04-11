using MyNetCore.Model;
using System;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 计划任务服务
    /// </summary>
    public interface ITaskJobServices : IBatchDIServicesTag
    {
        #region 添加任务

        /// <summary>
        /// 添加一个一次性任务，指定多少分钟后执行
        /// </summary>
        /// <param name="jobName">model</param>
        /// <returns>返回jobId，删除时需要</returns>
        Task<ApiResult> AddBackgroudJobDelayAsync(AddTaskJobModel model);

        /// <summary>
        /// 添加一个一次性任务，指定执行时间
        /// </summary>
        /// <param name="model">任务名称</param>
        /// <returns>返回jobId，删除时需要</returns>
        Task<ApiResult> AddBackgroudJobRunAtAsync(AddTaskJobModel model);

        /// <summary>
        /// 添加一个指定Corn的任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddRecurringCornJob(AddTaskJobModel model);

        #endregion

        #region 删除任务


        /// <summary>
        /// 根据jobid删除一个一次性任务
        /// </summary>
        /// <param name="jobId">添加时返回的JobId</param>
        /// <returns></returns>
        Task<ApiResult> RemoveBackgroundJobAsync(string jobId);

        /// <summary>
        /// 根据jobName删除周期任务
        /// </summary>
        /// <param name="jobName">添加时指定的JobName</param>
        /// <returns></returns>
        Task<ApiResult> RemoveRecurringJobAsync(string jobName);


        #endregion

    }
}
