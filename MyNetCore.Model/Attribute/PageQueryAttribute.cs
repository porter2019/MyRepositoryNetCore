using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageQueryAttribute : Attribute
    {
        public PageQueryAttribute() { }

        /// <summary>
        /// 只指定匹配类型
        /// </summary>
        /// <param name="type"></param>
        public PageQueryAttribute(PageQueryColumnMatchType type)
        {
            this.OperatoryType = type;
        }

        /// <summary>
        /// 忽略
        /// </summary>
        public bool IsIgnore { get; set; } = false;

        /// <summary>
        /// 列前缀，a.
        /// </summary>
        public string PrefixName { get; set; }

        /// <summary>
        /// 对应数据库的字段名称，如果为空，则跟列名一样
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 匹配类型
        /// </summary>
        public PageQueryColumnMatchType OperatoryType { get; set; }

    }

    /// <summary>
    /// where条件字段匹配类型
    /// </summary>
    public enum PageQueryColumnMatchType
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,

        /// <summary>
        /// like '%参数%'
        /// </summary>
        Like,

        /// <summary>
        /// like '参数%'
        /// </summary>
        LikeLeft,

        /// <summary>
        /// like '%参数'
        /// </summary>
        LikeRight,

        /// <summary>
        /// CHARINDEX('参数',UserName) > 0
        /// </summary>
        CharIndex,

        /// <summary>
        /// 值必须使用;分割,between and
        /// </summary>
        BetweenNumber,

        /// <summary>
        /// 时间Between;分割,between and
        /// </summary>
        BetweenDate,
    }

}
