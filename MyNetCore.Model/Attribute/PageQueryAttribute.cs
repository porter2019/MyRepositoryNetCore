namespace MyNetCore.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageQueryAttribute : Attribute
    {
        public PageQueryAttribute()
        { }

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
        /// 大于
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// bool类型使用，为True时给条件“=1”
        /// </summary>
        BoolWhenTrue,

        /// <summary>
        /// Int类型使用，当>0时给条件
        /// </summary>
        IntEqualWhenGreaterZero,

        /// <summary>
        /// Int类型使用，当>-1时给条件
        /// </summary>
        IntEqualWhenGreaterMinus,

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

        /// <summary>
        /// Int类型的in查询，值必须英文逗号分隔
        /// </summary>
        IntIn,

        /// <summary>
        /// Int类型的not in查询，值必须英文逗号分割
        /// </summary>
        IntNotIn,
    }
}