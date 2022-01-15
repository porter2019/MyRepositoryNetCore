using FreeSql.DataAnnotations;

namespace MyNetCore.Model
{
    /// <summary>
    /// FreeSql 列属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FsColumnAttribute : ColumnAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示名</param>
        public FsColumnAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示名</param>
        /// <param name="required">是否必填</param>
        public FsColumnAttribute(string displayName, bool required)
        {
            this.DisplayName = displayName;
            this.Required = required;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示名</param>
        /// <param name="stringLength">String字符串长度</param>
        public FsColumnAttribute(string displayName, int stringLength)
        {
            this.DisplayName = displayName;
            this.StringLength = stringLength;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName">显示名</param>
        /// <param name="required">是否必填</param>
        /// <param name="stringLength">String字符串长度</param>
        public FsColumnAttribute(string displayName, bool required, int stringLength)
        {
            this.DisplayName = displayName;
            this.Required = required;
            this.StringLength = stringLength;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 必须这样写，不然内存溢出
        /// </summary>
        private bool _IsPK;

        /// <summary>
        /// 是否主键(为True时同时自增)
        /// </summary>
        public bool IsPK
        {
            get { return _IsPK; }
            set
            {
                _IsPK = value;

                base.IsPrimary = _IsPK;
                base.IsIdentity = _IsPK;
            }
        }

        private bool _Required;

        /// <summary>
        /// 数据库字段不能为空
        /// </summary>
        public bool Required
        {
            get { return _Required; }
            set
            {
                _Required = value;
                base.IsNullable = !_Required;
            }
        }

        private bool _VerRequired;

        /// <summary>
        /// 验证必填
        /// </summary>
        public bool VerRequired
        {
            get { return _VerRequired; }
            set
            {
                _VerRequired = value;
            }
        }

        private bool _ReadOnly;

        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                base.CanInsert = !_ReadOnly;
                base.CanUpdate = !_ReadOnly;
            }
        }
    }
}