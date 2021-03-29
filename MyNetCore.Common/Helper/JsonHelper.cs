using Newtonsoft.Json;

namespace MyNetCore
{
    /// <summary>
    /// Json序列化
    /// </summary>
    public class JsonHelper
    {
        private static JsonSerializerSettings jsonSetting = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new Common.Config.NullToEmptyStringResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
        };

        /// <summary>
        /// 序列化对象为json字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(object value)
        {
            if (value == null) return "";

            return JsonConvert.SerializeObject(value, jsonSetting);
        }

        /// <summary>
        /// 序列化json字符串为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            if (json.IsNull()) return default;
            try
            {
                return JsonConvert.DeserializeObject<T>(json, jsonSetting);
            }
            catch
            {
                return default;
            }
        }

    }
}
