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
        /// 添加或修改用户
        /// </summary>
        /// <param entity=""></param>
        /// <returns></returns>
        Task<SysUser> Modify(SysUser entity);

        /// <summary>
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="userId">当前用户id</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        Task<bool> CheckLoginNameExists(int userId, string loginName);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="model"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<SysUser>> GetPageListAsync(Model.RequestModel.SysUserPageModel model, out long total);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Model.ResponseModel.LoginUserInfo> UserLogin(Model.RequestModel.SysUserLoginModel model);

    }
}
