/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统模块业务逻辑接口                                                    
*│　作    者：litdev                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2021-03-03 17:16:19                            
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
    /// 系统功能业务实现类
    /// </summary>
    public class SysModuleServices : BaseServices<SysModule, int>, ISysModuleServices
    {
        private readonly ISysModuleRepository _sysModuleRepository;

        public SysModuleServices(SysModuleRepository sysModuleRepository) : base(sysModuleRepository)
        {
            _sysModuleRepository = sysModuleRepository;
        }
    }
}
