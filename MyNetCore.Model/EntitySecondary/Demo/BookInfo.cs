namespace MyNetCore.Model.EntitySecondary
{
    /// <summary>
    /// 演示表
    /// </summary>
    [FsTable("演示表")]
    public class BookInfo : BaseEntityStandard
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int BookId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [FsColumn("标题", 255)]
        public string Title { get; set; }
    }
}