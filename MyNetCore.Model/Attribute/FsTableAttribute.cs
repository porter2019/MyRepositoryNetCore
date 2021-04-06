using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model
{
    /// <summary>
    /// FreeSql Table属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FsTableAttribute : TableAttribute
    {
        public FsTableAttribute(string displayName) { this.DisplayName = displayName; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 视图类，用于代码生成器
        /// </summary>
        public Type ViewClassName { get; set; }

    }
}
