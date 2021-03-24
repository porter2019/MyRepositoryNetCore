using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.Dto
{
    /// <summary>
    /// 数据库查询用户组的权限列表
    /// </summary>
    public class SysRolePermit
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 操作id
        /// </summary>
        public int PermitId { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string PermitName { get; set; }

        /// <summary>
        /// 操作别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 组是否有了该权限
        /// </summary>
        public bool IsChecked { get; set; }

    }

    /// <summary>
    /// 模块信息
    /// </summary>
    public class SysRoleModuleGroupModel
    {

        public SysRoleModuleGroupModel(string moduleName)
        {
            this.ModuleName = moduleName;
            this.HandlerList = new List<SysRoleHandlerGroupModel>();
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 功能列表
        /// </summary>
        public List<SysRoleHandlerGroupModel> HandlerList { get; set; }
    }

    /// <summary>
    /// 功能信息
    /// </summary>
    public class SysRoleHandlerGroupModel
    {

        public SysRoleHandlerGroupModel(string handlerName)
        {
            this.HandlerName = handlerName;
            this.PermitList = new List<SysRolePermit>();
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<SysRolePermit> PermitList { get; set; }

    }

}
