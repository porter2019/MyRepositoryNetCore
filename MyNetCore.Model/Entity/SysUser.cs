using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [FsTable("系统用户")]
    public class SysUser : BaseEntityStandard
    {
        [FsColumn("自增id", IsPK = true, Position = 1)]
        public int UserId { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [FsColumn("登录名", false, 200), ValidateRule(VerRequired = true, LengthRange = "2-20", ValidateType = ValidateType.AccountName)]
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FsColumn("用户名", false, 200), ValidateRule(VerRequired = true, LengthRange = "2-30")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [FsColumn("密码", false, 255)]
        public string Password { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [FsColumn("用户状态")]
        public bool Status { get; set; } = true;

    }

    /// <summary>
    /// 系统用户视图，包含所属组信息
    /// </summary>
    [FsTable("系统用户视图", Name = "SysUserView", DisableSyncStructure = true)]
    public class SysUserView : SysUser
    {
        /// <summary>
        /// 组信息
        /// </summary>
        [FsColumn("组信息", false, 255)]
        public string RoleInfo { get; set; }

        private int[] roleIdArray;
        /// <summary>
        /// 组id，用于前端还原显示
        /// </summary>
        public int[] RoleIdArray
        {
            get
            {
                if (RoleInfo.IsNull()) return roleIdArray;
                var tempArrList = new List<int>();
                foreach (var item in RoleInfo.SplitWithComma())
                {
                    var roleArr = item.SplitWithSemicolon();
                    if (roleArr.Length == 2)
                    {
                        var roleId = roleArr[0].ObjToInt(-1);
                        if (roleId != -1)
                            tempArrList.Add(roleId);
                    }
                }
                return tempArrList.ToArray();
            }
            set
            {
                roleIdArray = value;
            }
        }

    }
}
