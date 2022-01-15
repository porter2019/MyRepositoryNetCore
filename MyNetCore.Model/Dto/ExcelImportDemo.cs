using Magicodes.ExporterAndImporter.Core;
using System.ComponentModel.DataAnnotations;

namespace MyNetCore.Model.Dto
{
    /// <summary>
    /// Excel导入导出演示
    /// </summary>

    public class ExcelImportDemo
    {
        [ExporterHeader("编号")] //导出标题
        [ImporterHeader(IsIgnore = true)] //导入标题，忽略该字段
        public int Id { get; set; }

        [ExporterHeader("姓名")]
        [ImporterHeader(Name = "姓名")]
        [Required(ErrorMessage = "姓名不能为空"), MaxLength(50, ErrorMessage = "姓名字数超出最大限制,请修改!")]
        public string Name { get; set; }

        [ExporterHeader("数量")]
        [ImporterHeader(Name = "数量")]
        public int Num { get; set; }

        [ExporterHeader("可空数字")]
        [ImporterHeader(Name = "可空数字")]
        public int? Num2 { get; set; }

        [ExporterHeader("性别")]
        [ImporterHeader(Name = "性别")]
        [Required(ErrorMessage = "性别不能为空")]
        public Entity.DemoMainSex Sex { get; set; }

        [ExporterHeader("双精度")]
        [ImporterHeader(Name = "双精度")]
        [Required(ErrorMessage = "双精度不能为空")]
        public double Value { get; set; }

        [ExporterHeader("金额")]
        [ImporterHeader(Name = "金额")]
        [Required(ErrorMessage = "金额不能为空")]
        public decimal ValueDe { get; set; }

        [ExporterHeader("日期格式")]
        [ImporterHeader(Name = "日期格式")]
        [Required(ErrorMessage = "日期格式不能为空")]
        public DateTime Date1 { get; set; }
    }
}