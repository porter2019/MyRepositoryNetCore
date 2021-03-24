using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 角色组
    /// </summary>
    [FsTable("角色组")]
    public class SysRole : BaseEntityStandard
    {
        [FsColumn("自增id", IsPK = true, Position = 1)]
        public int RoleId { get; set; }

        /// <summary>
        /// 用户组名称
        /// </summary>
        [FsColumn("用户组名称", true, 50), ValidateRule(LengthRange = "1-30")]
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [FsColumn("描述", 500)]
        public string Description { get; set; }

        /// <summary>
        /// 是否超级管理组
        /// </summary>
        [FsColumn("是否超级管理组")]
        public bool IsSuper { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;


    }
}
