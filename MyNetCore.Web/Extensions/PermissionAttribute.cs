using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web
{
    /// <summary>
    /// 权限操作标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PermissionAttribute : ActionFilterAttribute
    {
        public PermissionAttribute()
        {
            this.OperationName = "";
        }

        public PermissionAttribute(string operationName)
        {
            this.OperationName = operationName;
        }

        public PermissionAttribute(string operationName, string aliasName)
        {
            this.OperationName = operationName;
            this.AliasName = aliasName;
        }

        /// <summary>
		/// 权限名称
		/// </summary>
		public string OperationName { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName { get; set; }

        private bool _anonymous = false;

        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        public bool Anonymous
        {
            get
            {
                return _anonymous;
            }
            set
            {
                _anonymous = value;
            }
        }

        private bool _autoCheck = true;

        /// <summary>
        /// 是否自动检查权限
        /// </summary>
        public bool AutoCheck
        {
            get
            {
                return _autoCheck;
            }
            set
            {
                _autoCheck = value;
            }
        }

    }
}
