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
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="userId">当前用户id</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public Task<bool> CheckLoginNameExists(int userId, string loginName)
        {
            if (userId > 0)
            {
                if (_sysUserRepository.Exists(p => p.UserId != userId && p.LoginName == loginName))
                    return Task.FromResult(true);
            }
            else
            {
                if (_sysUserRepository.Exists(p => p.LoginName == loginName))
                    return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param entity=""></param>
        /// <returns></returns>
        public async Task<SysUser> Modify(SysUser entity)
        {
            if (await CheckLoginNameExists(entity.UserId, entity.LoginName))
                throw new Exception($"登录名：【{entity.LoginName}】已存在");

            if (entity.Password.Equals("@@**@@") && entity.UserId > 0)
            {
                //不修改密码，获取旧密码
                var oldEntity = await _sysUserRepository.GetModelAsync(p => p.UserId == entity.UserId);
                entity.Password = oldEntity.Password;
            }
            else
            {
                entity.Password = entity.Password.EncryptMD5Encode();
            }
            return await _sysUserRepository.InsertOrUpdateAsync(entity);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="model"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<SysUser>> GetPageListAsync(Model.RequestModel.SysUserPageModel model, out long total)
        {
            model.PageInfo.Where = model.BuildPageSearchWhere();

            return _sysUserRepository.GetPageListAsync(model.PageInfo, out total);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Model.ResponseModel.LoginUserInfo> UserLogin(Model.RequestModel.SysUserLoginModel model)
        {
            var pwd = model.Password.EncryptMD5Encode();
            var entity = await _sysUserRepository.GetModelAsync(p => p.LoginName == model.Account && p.Password == pwd);
            if (entity == null) throw new NullReferenceException("用户名或密码有误");
            if (!entity.Status) throw new NullReferenceException("账户已被禁用");
            var responseModel = new Model.ResponseModel.LoginUserInfo()
            {
                LoginName = entity.LoginName,
                UserName = entity.UserName,
                FailureTime = DateTime.Now.AddDays(1).ToTimeStamp(),
            };
            responseModel.Token = $"{entity.LoginName}-{responseModel.FailureTime}";
            return responseModel;
        }

    }
}
