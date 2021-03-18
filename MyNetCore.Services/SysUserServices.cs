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
    /// 系统用户业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class SysUserServices : BaseServices<SysUser, int>, ISysUserServices
    {
        private readonly ISysUserRepository _sysUserRepository;

        public SysUserServices(SysUserRepository sysUserRepository) : base(sysUserRepository)
        {
            _sysUserRepository = sysUserRepository;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> UserLogin(Model.RequestModel.SysUserLoginModel model)
        {
            var pwd = model.Password.EncryptMD5Encode();
            var entity = await _sysUserRepository.GetModelAsync(p => p.LoginName == model.Account && p.Password == pwd);
            if (entity == null) return ApiResult.Failed("用户名或密码有误");
            if (!entity.Status) return ApiResult.Failed("账户已被禁用");
            var responseModel = new Model.ResponseModel.LoginUserInfo()
            {
                LoginName = entity.LoginName,
                UserName = entity.UserName,
                FailureTime = DateTime.Now.AddDays(1).ToTimeStamp(),
            };
            responseModel.Token = $"{entity.LoginName}-{responseModel.FailureTime}";
            return ApiResult.OK(responseModel);
        }

    }
}
