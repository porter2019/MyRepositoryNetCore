using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 查询系统用户分页列表所需参数
    /// </summary>
    public class SysUserViewPageModel : BaseRequestPageViewModel<Entity.SysUserView>
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex)]
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex)]
        public string UserName { get; set; }

        private string roleId = "";
        /// <summary>
        /// 用户组
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex, ColumnName = "RoleInfo")]
        public string RoleId
        {
            get
            {
                if (roleId.IsNotNull()) return $",{roleId};";
                else return roleId;
            }
            set
            {
                roleId = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string UpdatedDate { get; set; }

    }
}
