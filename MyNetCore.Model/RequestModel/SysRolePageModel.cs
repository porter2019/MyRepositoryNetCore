namespace MyNetCore.Model.RequestModel
{
    /// <summary>
    /// 查询系统用户组分页列表所需参数
    /// </summary>
    public class SysRolePageModel : BaseRequestPageViewModel<Entity.SysRole>
    {
        /// <summary>
        /// 用户组名
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.CharIndex)]
        public string RoleName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [PageQuery(PageQueryColumnMatchType.BetweenDate)]
        public string UpdatedDate { get; set; }
    }
}