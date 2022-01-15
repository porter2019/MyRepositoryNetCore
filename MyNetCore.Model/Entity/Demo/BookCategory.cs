using FreeSql.DataAnnotations;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 经典父子分类
    /// </summary>
    [FsTable("经典父子分类")]
    public class BookCategory : BaseEntityStandard
    {
        /// <summary>
        /// 分类id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int CategoryId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [FsColumn("标题", false, 200)]
        public string Title { get; set; }

        /// <summary>
        /// 排序数字，这里是string类型的，升序排列
        /// </summary>
        [FsColumn("排序数字", false, 50)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        [FsColumn("父级id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 上级类别名称
        /// </summary>
        [FsColumn("上级类别名称", false, 200)]
        public string ParentTitle { get; set; }

        /// <summary>
        /// 完整类别层级id，竖线分割
        /// </summary>
        [FsColumn("完整类别id", false, 500)]
        public string FullId { get; set; }

        /// <summary>
        /// 完整类别层级名称，竖线分割
        /// </summary>
        [FsColumn("完整类别名称", false, 4000)]
        public string FullTitle { get; set; }

        /// <summary>
        /// 完整类别层级排序，竖线分割
        /// </summary>
        [FsColumn("完整排序", false, 2000)]
        public string FullOrderNo { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [FsColumn("层级")]
        public int LevelNo { get; set; }

        /// <summary>
        /// 父级信息
        /// </summary>
        [Navigate(nameof(ParentId))]
        public BookCategory Parent { get; set; }

        /// <summary>
        /// 子列表
        /// </summary>
        [Navigate(nameof(ParentId))]
        public List<BookCategory> Childs { get; set; }
    }
}