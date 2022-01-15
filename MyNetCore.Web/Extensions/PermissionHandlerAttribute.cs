namespace MyNetCore.Web
{
    /// <summary>
    /// 需要权限控制的控制器加上此属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PermissionHandlerAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="handlerName">模块下的功能名称</param>
        /// <param name="aliasName">模块下的功能别名</param>
        /// <param name="orderNo">排序数字，降序排列，数字越大越靠前</param>
        public PermissionHandlerAttribute(string moduleName, string handlerName, string aliasName, int orderNo)
        {
            this.ModuleName = moduleName;
            this.HandlerName = handlerName;
            this.AliasName = aliasName;
            this.OrderNo = orderNo;
        }

        /// <summary>
        /// 模块名称(相当于功能所属组)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块下的功能名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 模块下的功能别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 排序数字，降序排列，数字越大越靠前
        /// </summary>
        public int OrderNo { get; set; } = 0;
    }
}