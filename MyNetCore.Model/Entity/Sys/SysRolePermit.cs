using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 用户组权限
    /// </summary>
    [FsTable("用户组权限")]
    public class SysRolePermit : BaseEntity
    {
        /// <summary>
        /// 用户组权限Id
        /// </summary>
        [FsColumn("用户组权限Id", IsPK = true, Position = 1)]
        public int RolePermitId { get; set; }

        /// <summary>
        /// 用户组Id
        /// </summary>
        [FsColumn("用户组Id", true)]
        public int RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [FsColumn("权限Id", true)]
        public int PermitId { get; set; }

    }
}
