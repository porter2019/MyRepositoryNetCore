using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyNetCore.IServices;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace MyNetCore.Web.API
{
    /// <summary>
    /// 系统接口
    /// </summary>
    [PermissionHandler("系统管理", "系统配置", "Sys", 10)]
    public class SysController : BaseOpenAPIController
    {
        private readonly ILogger<SysController> _logger;
        private readonly ISysUserServices _sysUserServices;
        private readonly ICacheServices _cache;

        public SysController(ILogger<SysController> logger, ISysUserServices sysUserServices, ICacheServices cache)
        {
            _logger = logger;
            _sysUserServices = sysUserServices;
            _cache = cache;
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
            return ApiResult.OK(_hostEnvironment.ContentRootPath);
        }

        /// <summary>
        /// 同步数据库结构
        /// </summary>
        /// <param name="_freeSql">自动从服务中获取注入的数据库操作对象</param>
        /// <param name="tableNames">同步指定表名，多个英文逗号分割，注意区分大小写，为空则全部同步</param>
        /// <returns></returns>
        [HttpGet, Route("db/sync")]
        [Permission("迁移数据库", "MigrateDB")]
        public ApiResult InitDB([FromServices] IFreeSql _freeSql, string tableNames)
        {
            var projectMainName = AppDomain.CurrentDomain.GetProjectMainName();

            //指定表明
            var tableNameList = tableNames.SplitWithComma().ToList();

            //实体所在程序集名称
            string assemblies = $"{projectMainName}.Model";
            //同步指定命名空间开头的实体
            string syncNameSpace = $"{projectMainName}.Model.Entity";
            if (string.IsNullOrWhiteSpace(assemblies) || string.IsNullOrWhiteSpace(syncNameSpace)) return ApiResult.Error();
            var assembliesArr = assemblies.SplitWithSemicolon();
            var syncNameSpaceArr = syncNameSpace.SplitWithSemicolon();
            if (assembliesArr.Length == 0 || syncNameSpaceArr.Length == 0) return ApiResult.Error();

            List<Type> initTypes = new List<Type>();
            foreach (var assem in assembliesArr)
            {
                Assembly assembly = Assembly.Load(assem.Trim());
                if (assembly == null) continue;
                foreach (var item in assembly.GetTypes().Where(p => p.IsSubclassOf(typeof(Model.BaseEntity)) && !p.IsAbstract))
                {
                    foreach (var nameSpace in syncNameSpaceArr)
                    {
                        if (initTypes.Contains(item)) continue;
                        if (item.FullName.StartsWith(nameSpace))
                        {
                            if (tableNameList.Count > 0)
                            {
                                if (tableNameList.Contains(item.Name)) initTypes.Add(item);
                            }
                            else
                            {
                                initTypes.Add(item);
                            }
                        }
                    }
                }
                //assembly.GetTypes().Where(p => p.IsSubclassOf(typeof(Entity.BaseEntity)) && !p.IsAbstract).ToList().ForEach(p => initTypes.Add(p));
            }

            _freeSql.CodeFirst.SyncStructure(initTypes.ToArray());

            return ApiResult.OK("同步数据库结构成功");
        }

        /// <summary>
        /// 同步权限
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("permission/sync")]
        public ApiResult SyncPermissionHandler([FromServices] IFreeSql _freeSql)
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
            var dbModuleList = _freeSql.Select<Model.Entity.SysModule>().ToList();

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
                    moduleId = (int)_freeSql.Insert(entityModule).ExecuteIdentity();
                    _logger.LogInformation($"【权限数据初始化】新增模块:{moduleName},数据编号:{moduleId}");
                }
                else moduleId = entityModule.Id;

                moduleIdList.Add(moduleId);
            }

            //删除冗余模块数据
            var delAffrows = _freeSql.Delete<Model.Entity.SysModule>().Where($"ModuleId not in ({moduleIdList.Join()})").ExecuteAffrows();
            _logger.LogInformation($"【权限数据初始化】清除冗余模块，删除{delAffrows}条");

            //重新查找数据库现有的模块
            dbModuleList = _freeSql.Select<Model.Entity.SysModule>().ToList();

            //功能集合
            var dbHandlerList = _freeSql.Select<Model.Entity.SysHandler>().ToList();
            //权限集合
            var dbPermitList = _freeSql.Select<Model.Entity.SysPermit>().ToList();

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
                    handerId = (int)_freeSql.Insert(newHandler).ExecuteIdentity();
                    _logger.LogInformation($"【权限数据初始化】新增功能：{handerName}，所属模块：{moduleName}，命名空间：{controllerFullName}");
                }
                else
                {

                    handerId = entityHandler.HandlerId;
                    newHandler.HandlerId = entityHandler.HandlerId;
                    newHandler.Version = entityHandler.Version;
                    var affrows = _freeSql.Update<Model.Entity.SysHandler>().SetSource(newHandler).ExecuteAffrows();
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
                        permitId = (int)(_freeSql.Insert(entityPermit).ExecuteIdentity());
                        _logger.LogInformation($"【权限数据初始化】新增权限：{opeartion}，所属功能:{handerName}，所属模块:{moduleName}");
                    }
                    else
                    {
                        permitId = entityPermit.PermitId;
                        if (!entityPermit.AliasName.Equals(opeartion.Value))
                        {
                            //如果改了别名，则只改别名就行了
                            _freeSql.Update<Model.Entity.SysPermit>(permitId).Set(p => p.AliasName == opeartion.Value).ExecuteAffrows();
                            _logger.LogInformation($"【权限数据初始化】修改修改权限别名：{opeartion}，新的别名：{opeartion.Value}，所属模块{moduleName},命名空间：{controllerFullName}");
                        }
                    }
                    permitIdList.Add(permitId);
                }
            }

            //删除冗余权限数据
            var delHandlerCount = _freeSql.Delete<Model.Entity.SysHandler>().Where($"HandlerId not in ({handlerIdList.Join()})").ExecuteAffrows();
            var delPermitCount = _freeSql.Delete<Model.Entity.SysPermit>().Where($"PermitId not in ({permitIdList.Join()})").ExecuteAffrows();
            var delRolePermitCount = _freeSql.Delete<Model.Entity.SysRolePermit>().Where($"PermitId not in ({permitIdList.Join()})").ExecuteAffrows();
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
