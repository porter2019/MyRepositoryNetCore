using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using MyNetCore.Model.Entity;

namespace MyNetCore.IRepository
{
    /// <summary>
    /// 系统用户仓储
    /// </summary>
    public interface ISysUserRepository : IBaseMyRepository<SysUser>
    {

        /// <summary>
        /// 注册用户
        /// </summary>
        Task Register();

        /// <summary>
        /// 根据用户名和密码获取信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<SysUser> Login(string loginName, string password);

    }
}
