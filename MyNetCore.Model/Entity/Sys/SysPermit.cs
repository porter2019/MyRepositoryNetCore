namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 权限关联
    /// </summary>
    [FsTable("权限关联")]
    public class SysPermit : BaseEntity
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        [FsColumn("权限Id", IsPK = true, Position = 1)]
        public int PermitId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [FsColumn("权限名称", true, 50)]
        public string PermitName { get; set; }

        /// <summary>
        /// 权限别名
        /// </summary>
        [FsColumn("权限别名", true, 50)]
        public string AliasName { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [FsColumn("功能Id", true)]
        public int HandlerId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;
    }
}