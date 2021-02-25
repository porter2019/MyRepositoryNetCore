using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web
{
    /// <summary>
    /// 需要权限控制的控制器加上此属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PermissionHandlerAttribute : Attribute
    {
        public PermissionHandlerAttribute(string moduleName, string handlerName, string aliasName, int orderNo)
        {
            this.ModuleName = moduleName;
            this.HandlerName = handlerName;
            this.AliasName = aliasName;
            this.OrderNo = orderNo;
        }


        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 功能别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int OrderNo { get; set; } = 0;

    }
}
