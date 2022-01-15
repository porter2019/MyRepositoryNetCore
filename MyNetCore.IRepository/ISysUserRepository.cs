namespace MyNetCore.IRepository
{
    /// <summary>
    /// 系统用户仓储
    /// </summary>
    public interface ISysUserRepository : IBaseMyRepository<SysUser>
    {
        /// <summary>
        /// 根据用户名和密码获取信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<SysUser> Login(string loginName, string password);
    }
}