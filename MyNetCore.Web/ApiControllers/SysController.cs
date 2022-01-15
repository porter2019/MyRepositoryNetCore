using System.Reflection;

namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 系统接口
    /// </summary>
    [PermissionHandler("系统管理", "系统配置", "sys", 10)]
    public class SysController : BaseOpenApiController
    {
        private readonly ILogger<SysController> _logger;
        private readonly ISysUserService _sysUserServices;
        private readonly ICacheService _cache;
        private readonly IDBSyncService _dbSyncServices;

        public SysController(ILogger<SysController> logger, ISysUserService sysUserServices, ICacheService cache, IDBSyncService dbSyncServices)
        {
            _logger = logger;
            _sysUserServices = sysUserServices;
            _cache = cache;
            _dbSyncServices = dbSyncServices;
        }

        /// <summary>
        /// 测试接口是否已通
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test")]
        public ApiResult Test()
        {
            //Model.RequestViewModel.SysUserPageModel model
            //var entity = await _sysUserServices.InsertAsync(new Model.Entity.SysUser()
            //{
            //    LoginName = "admin3",
            //    CreatedUserName = "手动",
            //    Password = "123",
            //    UserName = "d是rrr哈哈",
            //});
            //var entity = await _sysUserServices.GetModelAsync(p => p.UserId == 2);
            //var newEntity = entity.ShallowCopy<Model.Entity.SysUser>();
            //newEntity.UserName = "父类中返回子类对象";
            //int affrows = await _sysUserServices.UpdateOnlyChangeAsync(entity, newEntity);
            //return ApiResult.OK($"影响行数:{affrows}");

            //var list = await _sysUserServices.GetPageListAsync(model.PageOptions, out long total);

            //return ApiResult.OK(total, list);
            return ApiResult.OK(_hostEnvironment.EnvironmentName);
        }

        /// <summary>
        /// 同步主数据库结构
        /// </summary>
        /// <param name="_fsql">自动从服务中获取注入的数据库操作对象</param>
        /// <param name="tableNames">同步指定表名，多个英文逗号分割，注意区分大小写，为空则全部同步</param>
        /// <returns></returns>
        [HttpGet, Route("db/sync/main")]
        [Permission("迁移数据库", "MigrateDB", UnCheckWhenDevelopment = true)]
        public ApiResult InitMainDB([FromServices] IFreeSql<DBFlagMain> _fsql, string tableNames)
        {
            var projectMainName = AppDomain.CurrentDomain.GetProjectMainName();

            //指定表明
            var tableNameList = tableNames.SplitWithComma().ToList();

            //实体所在程序集名称
            string assemblies = $"{projectMainName}.Model";
            //同步指定命名空间开头的实体
            string syncNameSpace = $"{projectMainName}.Model.Entity";

            return _dbSyncServices.DBMigration(_fsql, assemblies, syncNameSpace, tableNames);
        }

        /// <summary>
        /// 初始化系统用户种子数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("db/sync/seeddata")]
        [Permission(Anonymous = true)]
        public async Task<ApiResult> InitSeedData()
        {
            await _sysUserServices.InitSeedDataAsync();
            return ApiResult.OK();
        }

        /// <summary>
        /// 同步次数据库结构
        /// </summary>
        /// <param name="_fsql">自动从服务中获取注入的数据库操作对象</param>
        /// <param name="tableNames">同步指定表名，多个英文逗号分割，注意区分大小写，为空则全部同步</param>
        /// <returns></returns>
        [HttpGet, Route("db/sync/secondary")]
        [Permission("迁移数据库", "MigrateDB", UnCheckWhenDevelopment = true)]
        public ApiResult InitDBFlagSecondaryDB([FromServices] IFreeSql<DBFlagSecondary> _fsql, string tableNames)
        {
            var projectMainName = AppDomain.CurrentDomain.GetProjectMainName();

            //指定表明
            var tableNameList = tableNames.SplitWithComma().ToList();

            //实体所在程序集名称
            string assemblies = $"{projectMainName}.Model";
            //同步指定命名空间开头的实体
            string syncNameSpace = $"{projectMainName}.Model.EntitySecondary";

            var result = _dbSyncServices.DBMigration(_fsql, assemblies, syncNameSpace, tableNames);
            if (result.IsOK())
            {
                //成功后通知更新视图
                var viewSqlRootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "ViewSQL");
                return _dbSyncServices.SqlViewSync(_fsql, viewSqlRootDir);
            }
            return result;
        }

        /// <summary>
        /// 同步数据库视图
        /// 执行Model\ViewSQL目录下的sql文件
        /// </summary>
        /// <param name="_fsql"></param>
        /// <returns></returns>
        [HttpGet, Route("view/sync/main")]
        [Permission("同步SQL视图", "SyncViewSql", UnCheckWhenDevelopment = true)]
        public ApiResult InitMainView([FromServices] IFreeSql<DBFlagMain> _fsql)
        {
            var viewSqlRootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "ViewSQL");

            return _dbSyncServices.SqlViewSync(_fsql, viewSqlRootDir);
        }

        /// <summary>
        /// 同步权限
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("permission/sync")]
        [Permission("同步权限", "PermissionAsync", UnCheckWhenDevelopment = true)]
        public ApiResult SyncPermissionHandler([FromServices] IFreeSql<DBFlagMain> _fsql)
        {
            //当前运行程序集
            Assembly assembly = Assembly.GetExecutingAssembly();

            Type[] types = assembly.GetExportedTypes();
            Func<Attribute[], bool> isAttribute = o =>
            {
                return o.OfType<PermissionHandlerAttribute>().Any();
            };
            Type[] controllerList = types.Where(o => isAttribute(Attribute.GetCustomAttributes(o, true)))
                .OrderBy(o => (o.GetCustomAttributes(typeof(PermissionHandlerAttribute), true)[0] as PermissionHandlerAttribute).OrderNo).ToArray();

            //模块Id集合
            List<int> moduleIdList = new List<int>();
            //功能Id集合
            List<int> handlerIdList = new List<int>();
            //权限Id集合
            List<int> permitIdList = new List<int>();

            //数据库已有的模块
            var dbModuleList = _fsql.Select<Model.Entity.SysModule>().ToList();

            //获取所有模块名称
            var registeredModuleList = controllerList
                                            .Select(p => p.GetCustomAttributes(typeof(PermissionHandlerAttribute), true)[0] as PermissionHandlerAttribute)
                                            .OrderByDescending(p => p.OrderNo)
                                            .ToList()
                                            .Select(p => p.ModuleName).Distinct().ToList();//去重
            foreach (var moduleName in registeredModuleList)
            {
                var moduleId = 0;
                var entityModule = dbModuleList.Where(p => p.ModuleName == moduleName).FirstOrDefault();
                if (entityModule == null)
                {
                    entityModule = new Model.Entity.SysModule(moduleName);
                    moduleId = (int)_fsql.Insert(entityModule).ExecuteIdentity();
                    _logger.LogInformation($"【权限数据初始化】新增模块:{moduleName},数据编号:{moduleId}");
                }
                else moduleId = entityModule.Id;

                moduleIdList.Add(moduleId);
            }

            //删除冗余模块数据
            var delAffrows = _fsql.Delete<Model.Entity.SysModule>().Where($"ModuleId not in ({moduleIdList.Join()})").ExecuteAffrows();
            _logger.LogInformation($"【权限数据初始化】清除冗余模块，删除{delAffrows}条");

            //重新查找数据库现有的模块
            dbModuleList = _fsql.Select<Model.Entity.SysModule>().ToList();

            //功能集合
            var dbHandlerList = _fsql.Select<Model.Entity.SysHandler>().ToList();
            //权限集合
            var dbPermitList = _fsql.Select<Model.Entity.SysPermit>().ToList();

            foreach (var controllerInfo in controllerList)
            {
                var handlerAttribute = (controllerInfo.GetCustomAttributes(typeof(PermissionHandlerAttribute), true)[0] as PermissionHandlerAttribute);
                if (handlerAttribute == null) continue;

                //控制器名称
                var controllerFullName = controllerInfo.FullName;
                var moduleName = handlerAttribute.ModuleName;
                if (moduleName.IsNull()) continue;
                var handerName = handlerAttribute.HandlerName;
                if (handerName.IsNull()) continue;
                var handerAliasName = handlerAttribute.AliasName;
                if (handerAliasName.IsNull()) continue;

                var moduleEntity = dbModuleList.Where(p => p.ModuleName == moduleName).FirstOrDefault();
                if (moduleEntity == null) continue;

                var newHandler = new Model.Entity.SysHandler()
                {
                    ModuleId = moduleEntity.ModuleId,
                    HandlerName = handerName,
                    AliasName = handerAliasName,
                    OrderNo = handlerAttribute.OrderNo,
                    RefController = controllerFullName
                };

                //功能
                var handerId = 0;
                var entityHandler = dbHandlerList.Where(p => p.RefController == controllerFullName).FirstOrDefault();
                if (entityHandler == null)
                {
                    handerId = (int)_fsql.Insert(newHandler).ExecuteIdentity();
                    _logger.LogInformation($"【权限数据初始化】新增功能：{handerName}，所属模块：{moduleName}，命名空间：{controllerFullName}");
                }
                else
                {
                    handerId = entityHandler.HandlerId;
                    newHandler.HandlerId = entityHandler.HandlerId;
                    newHandler.Version = entityHandler.Version;
                    var affrows = _fsql.Update<Model.Entity.SysHandler>().SetSource(newHandler).ExecuteAffrows();
                    if (affrows > 0)
                        _logger.LogInformation($"【权限数据初始化】修改功能：{handerName}，所属模块：{moduleName},命名空间：{controllerFullName}");
                }
                handlerIdList.Add(handerId);

                //权限
                var methodList = controllerInfo.GetMethods();
                //获取所有权限
                Dictionary<string, string> opeartionNameList = new Dictionary<string, string>();
                foreach (var methodInfo in methodList)
                {
                    var permissionAttr = methodInfo.GetCustomAttribute<PermissionAttribute>(true);
                    if (permissionAttr == null) continue;
                    if (permissionAttr.Anonymous) continue;

                    string[] operations = permissionAttr.OperationName.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in operations)
                    {
                        if (!opeartionNameList.ContainsKey(item)) opeartionNameList.Add(item, permissionAttr.AliasName);
                    }
                }

                foreach (var opeartion in opeartionNameList)
                {
                    int permitId = 0;
                    var entityPermit = dbPermitList.Where(p => p.HandlerId == handerId && p.PermitName == opeartion.Key).FirstOrDefault();// && p.AliasName == opeartion.Value
                    if (entityPermit == null)
                    {
                        entityPermit = new Model.Entity.SysPermit()
                        {
                            HandlerId = handerId,
                            PermitName = opeartion.Key,
                            AliasName = opeartion.Value
                        };
                        permitId = (int)(_fsql.Insert(entityPermit).ExecuteIdentity());
                        _logger.LogInformation($"【权限数据初始化】新增权限：{opeartion}，所属功能:{handerName}，所属模块:{moduleName}");
                    }
                    else
                    {
                        permitId = entityPermit.PermitId;
                        if (!entityPermit.AliasName.Equals(opeartion.Value))
                        {
                            //如果改了别名，则只改别名就行了
                            _fsql.Update<Model.Entity.SysPermit>(permitId).Set(p => p.AliasName == opeartion.Value).ExecuteAffrows();
                            _logger.LogInformation($"【权限数据初始化】修改修改权限别名：{opeartion}，新的别名：{opeartion.Value}，所属模块{moduleName},命名空间：{controllerFullName}");
                        }
                    }
                    permitIdList.Add(permitId);
                }
            }

            //删除冗余权限数据
            var delHandlerCount = _fsql.Delete<Model.Entity.SysHandler>().Where($"HandlerId not in ({handlerIdList.Join()})").ExecuteAffrows();
            var delPermitCount = _fsql.Delete<Model.Entity.SysPermit>().Where($"PermitId not in ({permitIdList.Join()})").ExecuteAffrows();
            var delRolePermitCount = _fsql.Delete<Model.Entity.SysRolePermit>().Where($"PermitId not in ({permitIdList.Join()})").ExecuteAffrows();
            _logger.LogInformation($"【权限数据初始化】清除冗余数据：功能清除{delHandlerCount}条，权限清除{delPermitCount}条，用户组权限清除{delRolePermitCount}条");

            return ApiResult.OK("同步完毕");
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet, Route("cache/get")]
        public ApiResult CacheGet(string key)
        {
            return ApiResult.OK(_cache.Get(key));
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet, Route("cache/add")]
        public ApiResult CacheAdd(string key, string value)
        {
            return ApiResult.OK(_cache.Add(key, value, 10));
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet, Route("cache/del")]
        public ApiResult CacheDel(string key)
        {
            return ApiResult.OK(_cache.Remove(key));
        }
    }
}