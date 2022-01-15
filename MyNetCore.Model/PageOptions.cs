using System.Reflection;

namespace MyNetCore
{
    /// <summary>
    /// 分页条件
    /// </summary>
    public class PageOptions<TEntity> //where TEntity : class//Model.BaseEntity
    {
        private int _pageIndex = 1;

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex < 1 ? 1 : _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        private int _pageSize = 10;

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize < 1 ? 10 : _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        private string _where { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where
        {
            get
            {
                return _where.IsNull() ? "" : _where;
            }
            set
            {
                _where = value.Trim();
                if (_where.IsNotNull())
                {
                    if (!_where.ToLower().StartsWith("and ")) _where = "and " + _where;
                }
            }
        }

        private string _orderBy;

        /// <summary>
        /// 排序条件
        /// </summary>
        public string OrderBy
        {
            get
            {
                string keyColumnName = "";
                //获取实体的主键
                var newType = typeof(TEntity);
                foreach (var item in newType.GetRuntimeProperties())
                {
                    var customAttr = item.CustomAttributes.Where(p => p.AttributeType == typeof(Model.FsColumnAttribute)).ToList().FirstOrDefault();
                    if (customAttr == null) continue;
                    var customAttrIsPK = customAttr.NamedArguments.Where(p => p.MemberName == "IsPK").FirstOrDefault();
                    if (customAttrIsPK.TypedValue.Value.ObjToBool()) { keyColumnName = item.Name; break; }
                }
                //如果没有设置排序
                if (_orderBy.IsNull())
                {
                    return keyColumnName.IsNotNull() ? keyColumnName + " DESC" : "";
                }
                else
                {
                    return _orderBy + "," + keyColumnName + " ASC";
                }
            }
            set
            {
                _orderBy = value;
            }
        }
    }
}