namespace MyNetCore
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public class GlobalVar
    {
        /// <summary>
        /// 跨域
        /// </summary>
        public const string AllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// Environment中当前环境的名称
        /// <code>Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")</code>
        /// </summary>
        public const string CurrentEnvironmentName = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// 配置文件中静态文件根目录key
        /// </summary>
        public const string StaticFilesDirectoryKey = "StaticFilesDirectory";

        /// <summary>
        /// 主站域名配置文件key
        /// </summary>
        public const string DomainUrlKey = "SysInfo:DomainName";

        /// <summary>
        /// token在报文头中的键名
        /// </summary>
        public const string AuthenticationTokenKey = "x-auth-token";
    }
}
