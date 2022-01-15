using System.ComponentModel.DataAnnotations;

namespace MyNetCore
{
    /// <summary>
	/// 通用的验证规则
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidateRuleAttribute : ValidationAttribute
    {
        /// <summary>
        /// 是否必填，针对的DTO,继承BaseEntity的无需指定此项
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// 验证使用，必填
        /// </summary>
        public bool VerRequired { get; set; } = false;

        /// <summary>
        /// 长度范围，示例：0-50
        /// </summary>
        public string LengthRange { get; set; }

        /// <summary>
        /// 值范围，示例：16-40
        /// </summary>
        public string NumberRange { get; set; }

        /// <summary>
        /// 针对DTO
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 数据类别
        /// </summary>
        public ValidateType ValidateType { get; set; }

        public ValidateRuleAttribute()
        {
            ValidateType = ValidateType.None;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //类型
            var valueType = validationContext.ObjectType.GetProperty(validationContext.MemberName).PropertyType;
            var valueBaseType = valueType.BaseType;

            //验证的属性
            string valName = validationContext.MemberName;
            //消息中的字段名称
            string valDisplayName = valName;
            //必填
            bool valRequired = false;

            var fsColumnAttr = validationContext.ObjectType.GetProperty(valName).CustomAttributes.ToList().Where(p => p.AttributeType == typeof(Model.FsColumnAttribute)).FirstOrDefault();
            if (fsColumnAttr != null) //FreeSQL实体类
            {
                var attrCount = fsColumnAttr.ConstructorArguments.Count;
                if (attrCount > 0) valDisplayName = fsColumnAttr.ConstructorArguments[0].Value.ObjToString();//取是否必填
                if (attrCount > 1) valRequired = fsColumnAttr.ConstructorArguments[1].Value.ObjToBool();
            }
            else
            {
                //ViewModel  DTO实体类
                valRequired = Required;
                if (!string.IsNullOrWhiteSpace(DisplayName)) valDisplayName = DisplayName;
            }

            if (VerRequired) valRequired = VerRequired;

            string inputText = value == null ? "" : Convert.ToString(value);

            if (valRequired) if (string.IsNullOrEmpty(inputText)) return new ValidationResult($"{valDisplayName}不能为空", new[] { valName });

            //枚举类型
            if (typeof(Enum) == valueBaseType)
            {
                if (!Enum.IsDefined(valueType, value)) return new ValidationResult($"{valDisplayName}的值不在枚举限定范围内", new[] { valName });
            }

            if (string.IsNullOrWhiteSpace(inputText)) return ValidationResult.Success;

            //验证时间
            if (typeof(DateTime) == valueType)
            {
                if (value.ObjToDate() == DateTime.MinValue) return new ValidationResult($"缺少{valDisplayName}", new[] { valName });
            }

            //验证长度范围
            if (!string.IsNullOrWhiteSpace(LengthRange))
            {
                var arr = LengthRange.SplitString("-");
                var ipnutTextByteLength = inputText.ByteLength();
                if (arr.Length == 2)
                {
                    if (ipnutTextByteLength < arr[0].ObjToInt() || ipnutTextByteLength > arr[1].ObjToInt())
                        return new ValidationResult($"{valDisplayName}长度范围在{LengthRange}之间", new[] { valName });
                }
                else
                {
                    var tempLength = LengthRange.ObjToInt(0);
                    if (tempLength <= 0)
                    {
                        return new ValidationResult($"{valDisplayName}LengthRange属性值有误", new[] { valName });
                    }
                    else
                    {
                        if (ipnutTextByteLength != tempLength)
                            return new ValidationResult($"{valDisplayName}长度为{tempLength}个字符", new[] { valName });
                    }
                }
            }

            switch (ValidateType)
            {
                case ValidateType.None:
                    break;

                case ValidateType.Email:
                    if (!Common.Helper.ValidateHelper.IsEmail(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的邮箱", new[] { valName });
                    break;

                case ValidateType.Date:
                    inputText = inputText.Split(' ')[0].Replace("/", "-");
                    if (!Common.Helper.ValidateHelper.IsDate(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的日期", new[] { valName });
                    break;

                case ValidateType.DateYear:
                    if (!Common.Helper.ValidateHelper.IsDateYear(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的年份", new[] { valName });
                    break;

                case ValidateType.DateMonth:
                    if (!Common.Helper.ValidateHelper.IsDateMonth(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的月份", new[] { valName });
                    break;

                case ValidateType.DateTime:
                    inputText = inputText.Replace("/", "-").Replace(" 0:", " 00:");
                    if (!Common.Helper.ValidateHelper.IsDateTime(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的时间日期", new[] { valName });
                    break;

                case ValidateType.Number:
                    if (!Common.Helper.ValidateHelper.IsNumeric(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的数字", new[] { valName });
                    break;

                case ValidateType.Money:
                    if (!Common.Helper.ValidateHelper.IsMoney(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的非负金额", new[] { valName });
                    break;

                case ValidateType.MoneyIncloudMinus:
                    if (!Common.Helper.ValidateHelper.IsMoneyIncloudMinus(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的金额", new[] { valName });
                    break;

                case ValidateType.IdNo:
                    if (!Common.Helper.ValidateHelper.IsIdCard(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的身份证号码", new[] { valName });
                    break;

                case ValidateType.CellPhone:
                    if (!Common.Helper.ValidateHelper.IsCellPhone(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的手机号", new[] { valName });
                    break;

                case ValidateType.WebURL:
                    if (!Common.Helper.ValidateHelper.IsWebUrl(inputText)) return new ValidationResult($"{valDisplayName}内容不是合法的网址", new[] { valName });
                    break;

                case ValidateType.AccountName:
                    if (!Common.Helper.ValidateHelper.IsAccountName(inputText)) return new ValidationResult($"{valDisplayName}只能由数字、字母、下划线组成", new[] { valName });
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(NumberRange))
            {
                var arr = NumberRange.SplitString("-");
                if (arr.Length != 2) return new ValidationResult($"{valDisplayName}LengthRange属性值有误", new[] { valName });
                var inputTextDecimal = inputText.ObjToDecimal(0);
                if (inputTextDecimal < arr[0].ObjToDecimal(0) || inputTextDecimal > arr[1].ObjToDecimal(0)) return new ValidationResult($"{valDisplayName}值范围在{NumberRange}之间", new[] { valName });
            }

            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// 验证类型
    /// </summary>
    public enum ValidateType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 邮箱
        /// </summary>
        Email,

        /// <summary>
        /// 日期 2019-12-12
        /// </summary>
        Date,

        /// <summary>
        /// 日期 年 2019
        /// </summary>
        DateYear,

        /// <summary>
        /// 日期 月 05
        /// </summary>
        DateMonth,

        /// <summary>
        /// 日期时间 2019-12-12 12:12:12
        /// </summary>
        DateTime,

        /// <summary>
        /// 数字
        /// </summary>
        Number,

        /// <summary>
        /// 金额，非负保留4位小数点
        /// </summary>
        Money,

        /// <summary>
        /// 金额，包含负数保留4位小数点
        /// </summary>
        MoneyIncloudMinus,

        /// <summary>
        /// 身份证号
        /// </summary>
        IdNo,

        /// <summary>
        /// 手机号
        /// </summary>
        CellPhone,

        /// <summary>
        /// 网页地址
        /// </summary>
        WebURL,

        /// <summary>
        /// 网站账户名，字母数字下划线组成
        /// </summary>
        AccountName
    }
}