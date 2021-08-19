using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 书籍信息
    /// </summary>
    [FsTable("书籍信息", ViewClassName = typeof(BookInfoView),VueModuleName = "demo")]
    public class BookInfo : BaseEntityStandard
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int BookId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [FsColumn("名称", true, 50), ValidateRule(LengthRange = "2-40")]
        public string Name { get; set; }

        /// <summary>
        /// 所属分类id
        /// </summary>
        [FsColumn("所属分类id")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [FsColumn("图片", false, 255)]
        public string ImagePath { get; set; }

        /// <summary>
        /// 图片完整访问路径
        /// </summary>
        public string ImagePathFull
        {
            get
            {
                if (ImagePath.IsNotNull()) return _config[GlobalVar.ConfigKeyPath_StaticFileDomainUrl] + ImagePath;
                else return "";
            }
        }


        /// <summary>
        /// 出版日期
        /// </summary>
        [FsColumn("出版日期"), ValidateRule(ValidateType = ValidateType.Date)]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

    }

    /// <summary>
    /// 书籍视图信息
    /// </summary>
    [FsTable("书籍视图信息", Name = "BookInfoView", DisableSyncStructure = true)]
    public class BookInfoView : BookInfo
    {
        /// <summary>
        /// 所属分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 所属分类完整层级关系
        /// </summary>
        public string CategoryFullId { get; set; }

    }

}
