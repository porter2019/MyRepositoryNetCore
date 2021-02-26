using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [FsTable("系统用户")]
    public class SysUser : BaseEntityStandard
    {
        [FsColumn("自增id", IsPK = true, Position = 1)]
        public int UserId { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [FsColumn("登录名", false, 200)]
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FsColumn("用户名", false, 200)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [FsColumn("密码", false, 255)]
        public string Password { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [FsColumn("用户状态")]
        public bool Status { get; set; } = true;

    }
}
