/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户组的用户业务逻辑接口                                                    
*│　作    者：杨习友                                          
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-27 13:03:08                            
*└──────────────────────────────────────────────────────────────┘
*/

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
    /// 用户组的用户业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class SysRoleUserServices : BaseServices<SysRoleUser, int>, ISysRoleUserServices
    {
        private readonly ISysRoleUserRepository _sysRoleUserRepository;

        public SysRoleUserServices(SysRoleUserRepository sysRoleUserRepository) : base(sysRoleUserRepository)
        {
            _sysRoleUserRepository = sysRoleUserRepository;
        }
    }
}
