namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 模块
    /// </summary>
    [FsTable("模块")]
    public class SysModule : BaseEntity
    {
        public SysModule()
        { }

        public SysModule(string moduleName)
        {
            this.ModuleName = moduleName;
        }

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
        /// 排序数字，降序排列
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