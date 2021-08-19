using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 演示主体
    /// </summary>
    [FsTable("演示主体", HaveItems = true, VueModuleName = "demo")]
    public class DemoMain : BaseEntityStandard
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int MainId { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        [FsColumn("标题", true, 50), ValidateRule(LengthRange = "2-40")]
        public string Title { get; set; }

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
        /// 数量
        /// </summary>
        [FsColumn("数量"), ValidateRule(ValidateType = ValidateType.Number)]
        public int Num { get; set; }

        /// <summary>
        /// 可空数字
        /// </summary>
        [FsColumn("可空数字"), ValidateRule(ValidateType = ValidateType.Number)]
        public int? Num2 { get; set; }

        /// <summary>
        /// 枚举
        /// </summary>
        [FsColumn("枚举")]
        public DemoMainSex Sex { get; set; }

        public string SexText
        {
            get
            {
                return Sex.GetEnumDescription();
            }
        }

        /// <summary>
        /// 双精度
        /// </summary>
        [FsColumn("双精度")]
        public double ValueD { get; set; }

        /// <summary>
        /// 金额类型
        /// </summary>
        [FsColumn("金额")]
        public decimal ValueDe { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        [FsColumn("日期格式"), ValidateRule(ValidateType = ValidateType.Date)]
        public DateTime Date1 { get; set; } = DateTime.Now;

        /// <summary>
        /// 日期时间格式
        /// </summary>
        [FsColumn("日期时间格式"), ValidateRule(ValidateType = ValidateType.DateTime)]
        public DateTime Date2 { get; set; } = DateTime.Now;

        /// <summary>
        /// 可控时间类型
        /// </summary>
        [FsColumn("可空时间"), ValidateRule(ValidateType = ValidateType.DateTime)]
        public DateTime? Date3 { get; set; }

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

        /// <summary>
        /// 图片列表
        /// </summary>
        public List<CommonAttach> ImageList { get; set; } = new List<CommonAttach>();

        /// <summary>
        /// 明细
        /// </summary>
        public List<DemoMainItem> Items { get; set; } = new List<DemoMainItem>();

    }

    /// <summary>
    /// 演示明细表
    /// </summary>
    [FsTable("演示明细表")]
    public class DemoMainItem : BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int ItemId { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [FsColumn("外键id")]
        public int MainId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [FsColumn("姓名", true, 30), ValidateRule(LengthRange = "2-30")]
        public string Name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [FsColumn("数量"), ValidateRule(ValidateType = ValidateType.Number)]
        public int Num { get; set; }

        /// <summary>
        /// 可空数字
        /// </summary>
        [FsColumn("可空数字"), ValidateRule(ValidateType = ValidateType.Number)]
        public int? Num2 { get; set; }

        /// <summary>
        /// 枚举
        /// </summary>
        [FsColumn("枚举")]
        public DemoMainSex Sex { get; set; }

        /// <summary>
        /// 金额类型
        /// </summary>
        [FsColumn("金额")]
        public decimal ValueDe { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [FsColumn("总金额")]
        public decimal TotalValue { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        [FsColumn("日期格式"), ValidateRule(ValidateType = ValidateType.Date)]
        public DateTime Date1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [FsColumn("备注", false, 200), ValidateRule(LengthRange = "0-200")]
        public string Remark { get; set; }

    }

    public enum DemoMainSex
    {
        [Description("未知")]
        未知,
        [Description("男")]
        男,
        [Description("女")]
        女,
    }
}
