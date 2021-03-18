using MyNetCore.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 系统用户业务类
    /// </summary>
    public interface ISysUserServices : IBaseServices<SysUser>
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UserLogin(Model.RequestModel.SysUserLoginModel model);

    }
}
