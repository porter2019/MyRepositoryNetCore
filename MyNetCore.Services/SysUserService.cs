namespace MyNetCore.Services
{
    /// <summary>
    /// 系统用户业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class SysUserService : BaseService<SysUser, int>, ISysUserService
    {
        private readonly ISysUserRepository _sysUserRepository;
        private readonly IFreeSql _fsq;
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;

        public SysUserService(ILogger<SysUserService> logger,
            IConfiguration config,
            IJwtService jwtService,
            ISysUserRepository sysUserRepository,
            IFreeSql<DBFlagMain> fsq,
            ISysRoleRepository sysRoleRepository) : base(sysUserRepository, logger)
        {
            _sysUserRepository = sysUserRepository;
            _fsq = fsq;
            _sysRoleRepository = sysRoleRepository;
            _config = config;
            _jwtService = jwtService;
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
        public async Task<SysUser> ModifyAsync(SysUserView entity)
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
            //工作单元
            using (var uow = _fsq.CreateUnitOfWork())
            {
                var userRepo = uow.GetRepository<SysUser>();
                userRepo.UnitOfWork = uow;
                var userRoleRepo = uow.GetRepository<SysRoleUser>();
                userRoleRepo.UnitOfWork = uow;

                var newEntity = await userRepo.InsertOrUpdateAsync(entity);
                var insertRoleList = new List<SysRoleUser>();
                //处理用户组
                if (entity.RoleIdArray != null)
                {
                    foreach (var roleId in entity.RoleIdArray)
                    {
                        insertRoleList.Add(new SysRoleUser()
                        {
                            RoleId = roleId,
                            UserId = newEntity.UserId,
                        });
                    }
                }
                //删除之前的
                await userRoleRepo.DeleteAsync(p => p.UserId == newEntity.Id);
                await userRoleRepo.InsertAsync(insertRoleList);

                uow.Commit();
            }
            entity.Password = "@@**@@";
            return entity;
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

            var exp_time = DateTime.Now.AddHours(_config.GetValue<double>("Jwt:TokenExpires")).ToTimeStamp();

            var responseModel = new Model.ResponseModel.LoginUserInfo()
            {
                LoginName = entity.LoginName,
                UserName = entity.UserName,
                FailureTime = exp_time,
            };
            if (_config.GetValue<bool>("Jwt:IsEnabled"))
            {
                var jsonData = JsonHelper.Serialize(responseModel);
                responseModel.Token = _jwtService.GenerateToken(new JwtTokenObject()
                {
                    exp = exp_time,
                    open_id = entity.UserId,
                    json_data = jsonData,
                });
            }
            else
            {
                responseModel.Token = entity.LoginName; //使用登录名做token，方便开发环境下开发
            }
            return responseModel;
        }

        /// <summary>
        /// 根据用户id获取所有的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetPermissionsByUserId(int userId)
        {
            var entityUser = GetModel(userId);
            if (entityUser == null) throw new NullReferenceException("用户不存在");
            if (!entityUser.Status) throw new NullReferenceException("该用户已被禁用");

            List<SysRole> roleList = await _sysRoleRepository.GetRoleListByUserId(userId);
            if (!roleList.Any()) throw new Exception("该用户没有归属的用户组");

            if (roleList.Any(p => p.IsSuper)) return new List<string>() { "ALL" };
            var roleIds = roleList.Select(p => p.RoleId).ToList().Join();
            return await _sysRoleRepository.GetPermissionsByRoleIds(roleIds);
        }

        /// <summary>
        /// 初始化种子数据，添加超级管理组和超级管理员，默认账号：admin 111111
        /// </summary>
        /// <returns></returns>
        public async Task InitSeedDataAsync()
        {
            var defaultRole = await _sysRoleRepository.GetModelAsync(p => p.IsSuper);
            if (defaultRole != null) throw new Exception("默认管理员组已存在");

            var defaultUser = await _sysUserRepository.GetCountAsync(p => p.UserId > 0);
            if (defaultUser != 0) throw new Exception("已有用户");

            using (var uow = _fsq.CreateUnitOfWork())
            {
                _sysRoleRepository.UnitOfWork = uow;
                _sysUserRepository.UnitOfWork = uow;
                var userRoleRepo = uow.GetRepository<SysRoleUser>();
                userRoleRepo.UnitOfWork = uow;

                var role = new SysRole()
                {
                    IsSuper = true,
                    RoleName = "超级管理员组",
                    CreatedUserId = 0,
                    CreatedUserName = "系统",
                    UpdatedUserId = 0,
                    UpdatedUserName = "系统",
                };

                await _sysRoleRepository.InsertAsync(role);

                var user = new SysUser()
                {
                    LoginName = "admin",
                    Password = "111111".EncryptMD5Encode(),
                    Status = true,
                    UserName = "管理员",
                    CreatedUserId = 0,
                    CreatedUserName = "系统",
                    UpdatedUserId = 0,
                    UpdatedUserName = "系统",
                };

                await _sysUserRepository.InsertAsync(user);

                var roleUser = new SysRoleUser()
                {
                    RoleId = role.RoleId,
                    UserId = user.UserId,
                };

                await userRoleRepo.InsertAsync(roleUser);

                uow.Commit();
            }
        }
    }
}