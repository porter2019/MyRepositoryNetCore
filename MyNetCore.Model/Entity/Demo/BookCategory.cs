using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 经典父子分类
    /// </summary>
    [FsTable("经典父子分类")]
    public class BookCategory : BaseEntityStandard
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int BookId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [FsColumn("标题", false, 200)]
        public string Title { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        [FsColumn("父级id")]
        public int? ParentId { get; set; }

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
