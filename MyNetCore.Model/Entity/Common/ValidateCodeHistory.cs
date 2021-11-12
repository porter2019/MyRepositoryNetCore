using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 验证码发送记录表
    /// </summary>
    [FsTable("验证码发送记录表")]
    public class ValidateCodeHistory : BaseEntity
    {
        /// <summary>
        /// 记录id
        /// </summary>
        [FsColumn("记录id", IsPK = true, Position = 1)]
        public int RecodeId { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        [FsColumn("唯一标识", true, 255)]
        public string UUID { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        [FsColumn("验证码类型")]
        public SMSType Type { get; set; }

        /// <summary>
        /// 验证码类型名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return Type.GetEnumDescription();
            }
        }

        /// <summary>
        /// 收件人，手机号或邮箱
        /// </summary>
        [FsColumn("收件人", true, 200)]
        public string Address { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        [FsColumn("发送内容", false, 2000)]
        public string Body { get; set; }

        /// <summary>
        /// 是否发送成功
        /// </summary>
        [FsColumn("是否发送成功")]
        public bool IsSendOK { get; set; }

        /// <summary>
        /// 发送返回值
        /// </summary>
        [FsColumn("发送返回值", false, 2000)]
        public string Result { get; set; }

        /// <summary>
        /// 忽略
        /// </summary>
        [JsonIgnore]
        new static List<CommonAttach> Attachs => null;

    }

    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum SMSType
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        CellPhone,
    }

}
