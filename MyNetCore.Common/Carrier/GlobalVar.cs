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
        /// token在报文头中的键名
        /// </summary>
        public const string AuthenticationTokenKey = "x-auth-token";
    }
}
