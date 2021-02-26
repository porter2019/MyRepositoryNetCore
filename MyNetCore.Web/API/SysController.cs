using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNetCore.Web.API
{
    /// <summary>
    /// 系统接口
    /// </summary>
    [PermissionHandler("系统管理", "系统配置", "Sys", 10)]
    public class SysController : BaseOpenAPIController
    {
        private readonly ILogger<SysController> _logger;

        public SysController(ILogger<SysController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 测试接口是否已通
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test")]
        public ApiResult Test()
        {
            _logger.LogInformation("OK");
            return ApiResult.OK(AppSettings.Get<string>("DBContexts:SqlServer:ConnectionString"));//"DBContexts", "SqlServer", "LazyLoading"
        }

        /// <summary>
        /// 同步数据库结构
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("db/sync")]
        public ApiResult InitDB()
        {
            var projectMainName = AppDomain.CurrentDomain.GetProjectMainName();

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
                        if (item.FullName.StartsWith(nameSpace)) initTypes.Add(item);
                    }
                }
                //assembly.GetTypes().Where(p => p.IsSubclassOf(typeof(Entity.BaseEntity)) && !p.IsAbstract).ToList().ForEach(p => initTypes.Add(p));
            }

            _freeSql.CodeFirst.SyncStructure(initTypes.ToArray());

            return ApiResult.OK("同步数据库结构成功");
        }

    }
}
