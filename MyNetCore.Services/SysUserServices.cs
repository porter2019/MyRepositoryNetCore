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
    public class SysUserServices : BaseServices<SysUser, int>, ISysUserServices
    {
        private readonly ISysUserRepository _sysUserRepository;

        public SysUserServices(SysUserRepository sysUserRepository) : base(sysUserRepository)
        {
            _sysUserRepository = sysUserRepository;
        }

        
    }
}
