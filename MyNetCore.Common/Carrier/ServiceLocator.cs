namespace MyNetCore
{
    /// <summary>
    /// 所有注入的对象
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// 所有注入的对象实例
        /// </summary>
        public static IServiceProvider Instance { get; set; }
    }
}