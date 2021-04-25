using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model
{
    /// <summary>
    /// 所有Post对象需要集成的基类
    /// <code>方便在filter中自动注入当前登录的用户信息，这些信息在业务层中可能会需要用到</code>
    /// </summary>
    public class BaseRequestPageViewModel<TEntity> : BaseRequestPostViewModel where TEntity : class, new()
    {

        /// <summary>
        /// 分页数据
        /// </summary>
        public PageOptions<TEntity> PageInfo { get; set; }

        /// <summary>
        /// 创建分页查询的where条件
        /// </summary>
        /// <returns></returns>
        public virtual string BuildPageSearchWhere()
        {
            var currentType = this.GetType();

            StringBuilder sbWhere = new StringBuilder();

            foreach (var itemType in currentType.GetProperties())
            {
                var quarrAttrArray = itemType.GetCustomAttributes(typeof(PageQueryAttribute), true);
                if (!quarrAttrArray.Any()) continue;
                var queryAttr = quarrAttrArray[0] as PageQueryAttribute;
                if (queryAttr.IsIgnore) continue;

                var filedName = itemType.Name;
                var filedValue = itemType.GetValue(this, null);
                if (filedValue == null) continue;
                if (filedValue.ToString().IsNull()) continue;
                var fileValueChar = "";

                var columnName = queryAttr.ColumnName.IsNull() ? $"[{filedName}]" : $"[{queryAttr.ColumnName}]";
                var sqlColumnName = queryAttr.PrefixName.IsNull() ? columnName : queryAttr.PrefixName + "." + columnName;

                var propType = itemType.PropertyType;
                if (propType == typeof(System.String))
                {
                    fileValueChar = "'";
                }
                else if (propType == typeof(System.Boolean))
                {
                    filedValue = filedValue.ToString().EqualsIgnoreCase("true") ? 1 : 0;
                }

                switch (queryAttr.OperatoryType)
                {
                    case PageQueryColumnMatchType.Equal:
                        sbWhere.Append($" and {sqlColumnName} = {fileValueChar}{filedValue}{fileValueChar}");
                        break;
                    case PageQueryColumnMatchType.NotEqual:
                        sbWhere.Append($" and {sqlColumnName} != {fileValueChar}{filedValue}{fileValueChar}");
                        break;
                    case PageQueryColumnMatchType.GreaterThan:
                        sbWhere.Append($" and {sqlColumnName} > {fileValueChar}{filedValue}{fileValueChar}");
                        break;
                    case PageQueryColumnMatchType.GreaterThanOrEqual:
                        sbWhere.Append($" and {sqlColumnName} >= {fileValueChar}{filedValue}{fileValueChar}");
                        break;
                    case PageQueryColumnMatchType.LessThan:
                        sbWhere.Append($" and {sqlColumnName} < {fileValueChar}{filedValue}{fileValueChar}");
                        break;
                    case PageQueryColumnMatchType.LessThanOrEqual:
                        sbWhere.Append($" and {sqlColumnName} <= {fileValueChar}{filedValue}{fileValueChar}");
                        break;
                    case PageQueryColumnMatchType.BoolWhenTrue:
                        if (filedValue.ObjToInt() == 1)
                        {
                            sbWhere.Append($" and {sqlColumnName} = 1");
                        }
                        break;
                    case PageQueryColumnMatchType.IntEqualWhenGreaterZero:
                        if (filedValue.ObjToInt() > 0)
                        {
                            sbWhere.Append($" and {sqlColumnName} = {filedValue}");
                        }
                        break;
                    case PageQueryColumnMatchType.Like:
                        sbWhere.Append($" and {sqlColumnName} like '%{filedValue}%'");
                        break;
                    case PageQueryColumnMatchType.LikeLeft:
                        sbWhere.Append($" and {sqlColumnName} like '{filedValue}%'");
                        break;
                    case PageQueryColumnMatchType.LikeRight:
                        sbWhere.Append($" and {sqlColumnName} like '%{filedValue}'");
                        break;
                    case PageQueryColumnMatchType.CharIndex:
                        sbWhere.Append($" and CharIndex('{filedValue}',{sqlColumnName}) > 0");
                        break;
                    case PageQueryColumnMatchType.BetweenNumber:
                        var tempArr = filedValue.ToString().SplitWithSemicolon();
                        if (tempArr.Length != 2) throw new Exception("Between条件下值的格式必须使用英文分号分割");
                        sbWhere.Append($" and {sqlColumnName} between {tempArr[0]} and {tempArr[1]}");
                        break;
                    case PageQueryColumnMatchType.BetweenDate:
                        var tempArr2 = filedValue.ToString().SplitWithSemicolon();
                        if (tempArr2.Length != 2) throw new Exception("Between条件下值的格式必须使用英文分号分割");
                        sbWhere.Append($" and {sqlColumnName} between '{tempArr2[0]}' and '{tempArr2[1]} 23:59:59'");
                        break;
                    default:
                        break;
                }

            }

            return sbWhere.ToString();

        }

    }
}
