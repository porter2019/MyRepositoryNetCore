using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 查询系统用户组分页列表所需参数
    /// </summary>
    public class SysRolePageModel : BaseRequestViewModel
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public PageOptions<Entity.SysRole> PageOptions { get; set; }

        /// <summary>
        /// 其他参数
        /// </summary>
        public int Type { get; set; }

    }
}
