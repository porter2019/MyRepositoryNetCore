using FreeSql;
using MyNetCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 系统用户仓储实现
    /// </summary>
    public class SysUserRepository : BaseMyRepository<SysUser, int>, ISysUserRepository
    {

        public SysUserRepository(ILogger<SysUserRepository> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {

        }

        /// <summary>
        /// 根据用户名和密码获取信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<SysUser> Login(string loginName, string password)
        {
            return _fsql.Select<SysUser>().Where(p => p.LoginName == loginName && p.Password == password).ToOneAsync();
        }
    }
}
