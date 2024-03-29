﻿namespace MyNetCore.IServices
{
    /// <summary>
    /// 系统用户业务类
    /// </summary>
    public interface ISysUserService : IBaseService<SysUser>
    {
        /// <summary>
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="userId">当前用户id</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        Task<bool> CheckLoginNameExists(int userId, string loginName);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Model.ResponseModel.LoginUserInfo> UserLogin(Model.RequestModel.SysUserLoginModel model);

        /// <summary>
        /// 根据用户id获取所有的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetPermissionsByUserId(int userId);

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<SysUser> ModifyAsync(SysUserView entity);

        /// <summary>
        /// 初始化种子数据，添加超级管理组和超级管理员，默认账号：admin 111111
        /// </summary>
        /// <returns></returns>
        Task InitSeedDataAsync();
    }
}