using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;

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

        public SysController(ILogger<SysController> logger, ISysUserServices sysUserServices)
        {
            _logger = logger;
            _sysUserServices = sysUserServices;
        }

        /// <summary>
        /// 测试接口是否已通
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("test")]
        public async Task<ApiResult> Test(Model.RequestViewModel.SysUserPageModel model)
        {
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

            var list = await _sysUserServices.GetPageListAsync(model.PageOptions, out long total);

            return ApiResult.OK(total, list);
        }

        /// <summary>
        /// 同步数据库结构
        /// </summary>
        /// <param name="tableNames">同步指定表名，多个英文逗号分割，注意区分大小写，为空则全部同步</param>
        /// <returns></returns>
        [HttpGet, Route("db/sync")]
        [Permission("迁移数据库", "MigrateDB")]
        public ApiResult InitDB(string tableNames)
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

    }
}
