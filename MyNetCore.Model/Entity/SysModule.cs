using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 模块
    /// </summary>
    [FsTable("模块")]
    public class SysModule : BaseEntity
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        [FsColumn("模块Id", IsPK = true, Position = 1)]
        public int ModuleId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [FsColumn("模块名称", true, 50)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;
    }
}
