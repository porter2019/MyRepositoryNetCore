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

        /// <summary>
        /// 是否有明细，用于代码生成
        /// </summary>
        public bool HaveItems { get; set; } = false;

        /// <summary>
        /// Vue所属的模块名，用于代码生成器，路由、vue文件存放的目录
        /// </summary>
        public string VueModuleName { get; set; } = "auto";

    }
}
