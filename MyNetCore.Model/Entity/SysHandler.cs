using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 功能
    /// </summary>
    [FsTable("功能")]
    public class SysHandler:BaseEntity
    {
        /// <summary>
        /// 功能Id
        /// </summary>
        [FsColumn("功能Id", IsPK = true, Position = 1)]
        public int HandlerId { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>
        [FsColumn("模块Id", true)]
        public int ModuleId { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [FsColumn("功能名称", true, 50)]
        public string HandlerName { get; set; }

        /// <summary>
        /// 功能别名
        /// </summary>
        [FsColumn("功能别名", true, 50)]
        public string AliasName { get; set; }

        /// <summary>
        /// 关联控制器
        /// </summary>
        [FsColumn("关联控制器", true, 500)]
        public string RefController { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        [FsColumn("排序数字", true)]
        public int OrderNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;
    }
}
