using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 测试
    /// </summary>
    [FsTable("测试", ViewClassName = typeof(TestCodeView))]
    public class TestCode : BaseEntityStandard
    {

        /// <summary>
        /// 主键id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int TestId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [FsColumn("标题", false, 255), ValidateRule(LengthRange = "1-255")]
        public string Title { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [FsColumn("年龄")]
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [FsColumn("性别")]
        public DemoMainSex Sex { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [FsColumn("数量"), ValidateRule(ValidateType = ValidateType.Number)]
        public int Num { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [FsColumn("金额")]
        public double Value { get; set; }

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
                if (ImagePath.IsNotNull()) return AppSettings.Get(GlobalVar.DomainUrlKey) + ImagePath;
                else return "";
            }
        }

        /// <summary>
        /// 日期格式
        /// </summary>
        [FsColumn("日期格式"), ValidateRule(ValidateType = ValidateType.Date)]
        public DateTime Date1 { get; set; } = DateTime.Now;

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;

        /// <summary>
        /// 富文本
        /// </summary>
        [FsColumn("富文本", false, DbType = "text"), ValidateRule(LengthRange = "0-8000")]
        public string HtmlText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [FsColumn("备注", false, 200), ValidateRule(LengthRange = "0-200")]
        public string Remark { get; set; }

    }

    /// <summary>
    /// 测试的视图
    /// </summary>
    [FsTable("测试视图", Name = "TestCodeView", DisableSyncStructure = true)]
    public class TestCodeView : TestCode
    {

    }

}
