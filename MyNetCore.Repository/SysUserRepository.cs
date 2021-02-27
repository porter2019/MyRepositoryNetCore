using FreeSql;
using MyNetCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 系统用户仓储实现
    /// </summary>
    public class SysUserRepository : BaseMyRepository<SysUser, int>, ISysUserRepository
    {
        private readonly IFreeSql _freeSql;

        public SysUserRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }

        /// <summary>
        /// 注册
        /// </summary>
        public Task Register()
        {
            return Task.Run(() => System.Diagnostics.Trace.Write("用户仓储中数据库操作"));
        }

        /// <summary>
        /// 根据用户名和密码获取信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<SysUser> Login(string loginName, string password)
        {
            return Task.Run(() => FindAsync(1));
        }
    }
}
