using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MyNetCore.Web
{
    /// <summary>
    /// 权限操作标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PermissionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 权限操作标识
        /// </summary>
        public PermissionAttribute() { }

        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        /// <param name="anonymous"></param>
        public PermissionAttribute(bool anonymous)
        {
            this.Anonymous = anonymous;
        }

        /// <summary>
        /// 只指定操作名
        /// </summary>
        /// <param name="operationName"></param>
        public PermissionAttribute(string operationName)
        {
            this.OperationName = operationName;
        }

        /// <summary>
        /// 指定操作名和别名
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="aliasName"></param>
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
            get { return _anonymous; }
            set { _anonymous = value; }
        }

        private bool _autoCheck = true;

        /// <summary>
        /// 是否自动检查权限
        /// </summary>
        public bool AutoCheck
        {
            get { return _autoCheck; }
            set { _autoCheck = value; }
        }

        private bool _unCheckWhenDevelopment = false;

        /// <summary>
        /// 开发环境下跳过权限验证
        /// </summary>
        public bool UnCheckWhenDevelopment
        {
            get { return _unCheckWhenDevelopment; }
            set { _unCheckWhenDevelopment = value; }
        }


    }
}
