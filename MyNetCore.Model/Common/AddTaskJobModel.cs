namespace MyNetCore.Model
{
    /// <summary>
    /// Http TaskJob添加所需参数
    /// </summary>
    public class AddTaskJobModel
    {
        /// <summary>
        /// BackgroudJob，指定多少分钟后执行
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="url">任务访问的url</param>
        /// <param name="method">请求方式，Get/Post</param>
        /// <param name="delayFromMinutes">指定多少分钟后执行</param>
        public AddTaskJobModel(string jobName, string url, string method, int delayFromMinutes)
        {
            this.JobName = jobName;
            this.Url = url;
            this.Method = method;
            this.DelayFromMinutes = delayFromMinutes;
        }

        /// <summary>
        /// BackgroudJob，指定运行时间
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="url">任务访问的url</param>
        /// <param name="method">请求方式，Get/Post</param>
        /// <param name="runAt">指定运行时间</param>
        public AddTaskJobModel(string jobName, string url, string method, DateTime runAt)
        {
            this.JobName = jobName;
            this.Url = url;
            this.Method = method;
            this.RunAt = runAt;
        }

        /// <summary>
        /// RecurringJob，指定Corn表达式
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="url">任务访问的url</param>
        /// <param name="method">请求方式，Get/Post</param>
        /// <param name="corn">Corn表达式</param>
        public AddTaskJobModel(string jobName, string url, string method, string corn)
        {
            this.JobName = jobName;
            this.Url = url;
            this.Method = method;
            this.Corn = corn;
        }

        /// <summary>
        /// 任务名称，如果是一次性后台任务，则仅仅是标识使用，如果是Recurring任务，则删除时需要这个同样的JobName
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 访问的url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 访问方式，Get/Post
        /// </summary>
        public string Method { get; set; }

        private int _DelayFromMinutes = 0;

        /// <summary>
        /// BackgroudJob，指定多少分钟后执行
        /// </summary>
        public int DelayFromMinutes
        {
            get
            {
                return _DelayFromMinutes < 0 ? 0 : _DelayFromMinutes;
            }
            set
            {
                _DelayFromMinutes = value;
            }
        }

        private DateTime _RunAt = DateTime.Now;

        /// <summary>
        /// BackgroudJob，指定运行时间
        /// </summary>
        public DateTime RunAt
        {
            get
            {
                return _RunAt < DateTime.Now ? DateTime.Now : _RunAt;
            }
            set
            {
                _RunAt = value;
            }
        }

        /// <summary>
        /// RecurringJob，指定Corn表达式，支持 6位数的Cron表达式(支持到秒级)，F12查看常用示例
        /// <code>执行周期的问题：https://github.com/yuzd/Hangfire.HttpJob/issues/66 </code>
        /// <code>0 15 10 ? * *    每天上午10:15触发 </code>
        /// <code>0 0/5 14 * * ?   在每天下午2点到下午2:55期间的每5分钟触发 </code>
        /// <code>0 0 10,14,16 * * ?   每天上午10点、下午2点、4点触发 </code>
        /// <code>0 0 12 ? * WED   每个星期三中午12点触发 </code>
        /// <code>0 15 10 ? * MON-FRI    周一至周五的上午10:15触发 </code>
        /// <code>0 25 10 15 * ?   每月15日上午10:25触发 </code>
        /// <code>0 15 10 L * ?    每月最后一日的上午10:15触发 </code>
        /// <code>更多参考：https://www.cnblogs.com/yanghj010/p/10875151.html</code>
        /// </summary>
        public string Corn { get; set; }
    }
}