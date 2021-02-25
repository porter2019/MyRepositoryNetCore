using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 用户组的用户
    /// </summary>
    [FsTable("用户组的用户")]
    public class SysRoleUser : BaseEntity
    {
        /// <summary>
        /// 用户组用户Id
        /// </summary>
        [FsColumn("用户组用户Id", IsPK = true, Position = 1)]
        public int RoleUserId { get; set; }

        /// <summary>
        /// 用户组Id
        /// </summary>
        [FsColumn("用户组Id", true)]
        public int RoleId { set; get; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [FsColumn("用户Id", true)]
        public int UserId { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;
    }
}
