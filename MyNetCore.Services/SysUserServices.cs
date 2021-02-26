using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using MyNetCore.Model.Entity;

namespace MyNetCore.Services
{
    /// <summary>
    /// 系统用户业务逻辑
    /// </summary>
    public class SysUserServices : ISysUserServices
    {
        private readonly ISysUserRepository _sysUserRepository;

        public SysUserServices(SysUserRepository sysUserRepository)
        {
            _sysUserRepository = sysUserRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SysUser> Login(string loginName, string password)
        {
            return await Task.Run(() => _sysUserRepository.Login(loginName, password));
        }

        /// <summary>
        /// 系统用户注册
        /// </summary>
        public async Task Register()
        {
            await Task.Run(() =>
            {
                _sysUserRepository.Register();
                System.Diagnostics.Trace.Write("这里处理注册用户的业务逻辑");
            });

        }
    }
}
