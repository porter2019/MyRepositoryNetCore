using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 用户登录所需参数
    /// </summary>
    public class SysUserLoginModel
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Remember { get; set; }

    }
}
