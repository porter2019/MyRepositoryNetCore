using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Common.Config
{
    /// <summary>
    /// NewtonsoftJson序列化配置，将为null的string转为空字符串
    /// </summary>
    public class NullToEmptyStringResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberSerialization">序列化成员</param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //首字母禁止转小写
            this.NamingStrategy = null;
            return type.GetProperties().Select(c =>
            {
                var jsonProperty = base.CreateProperty(c, memberSerialization);
                jsonProperty.ValueProvider = new NullToEmptyStringValueProvider(c);
                return jsonProperty;
            }).ToList();
        }
    }

    public class NullToEmptyStringValueProvider : IValueProvider
    {
        private readonly PropertyInfo _memberInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memberInfo"></param>
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _memberInfo = memberInfo;
        }

        /// <summary>
        /// 获取Value
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public object GetValue(object target)
        {
            var result = _memberInfo.GetValue(target);
            if (_memberInfo.PropertyType == typeof(string) && result == null)
                result = string.Empty;

            return result;
        }

        /// <summary>
        /// 设置Value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue(object target, object value)
        {
            _memberInfo.SetValue(target, value);
        }
    }
}
