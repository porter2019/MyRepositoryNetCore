using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyNetCore.Model
{
    /// <summary>
    /// 所有Post对象需要继承的基类
    /// <code>方便在filter中自动注入当前登录的用户信息，这些信息在业务层中可能会需要用到</code>
    /// </summary>
    public class BaseRequestPostViewModel
    {
        /// <summary>
        /// 当前操作的用户id
        /// </summary>
        [JsonIgnore]
        public int CurrentUserId { get; set; }

        /// <summary>
        /// 当前操作的用户名
        /// </summary>
        [JsonIgnore]
        public string CurrentUserName { get; set; }
    }
}
