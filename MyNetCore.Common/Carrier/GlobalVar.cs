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
        public const string ConfigKeyPath_StaticFilesDirectoryKey = "StaticFilesDirectory";

        /// <summary>
        /// 静态文件统一访问域名Key
        /// </summary>
        public const string ConfigKeyPath_StaticFileDomainUrl = "DomainUrl:StaticFileDomainUrl";

        /// <summary>
        /// token在报文头中的键名
        /// </summary>
        public const string ConfigKeyPath_AuthenticationTokenKey = "Jwt:TokenKey";
    }
}
