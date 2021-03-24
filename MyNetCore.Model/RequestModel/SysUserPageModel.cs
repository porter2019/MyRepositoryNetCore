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
    public class SysUserPageModel : BaseRequestViewModel
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public PageOptions<Entity.SysUser> PageInfo { get; set; }

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

        /// <summary>
        /// 创建时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string CreatedDate { get; set; }

    }
}
