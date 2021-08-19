using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNetCore.IServices;

namespace MyNetCore.Services
{
    /// <summary>
    /// 数据库同步相关服务
    /// </summary>
    [ServiceLifetime(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
    public class DBSyncService : IDBSyncService
    {
        private readonly ILogger _logger;

        public DBSyncService(ILogger<DBSyncService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 数据库CodeFrist迁移
        /// </summary>
        /// <param name="fsql"></param>
        /// <param name="assemblies">实体程序集名称</param>
        /// <param name="syncNameSpace">同步指定命名空间开头的实体类</param>
        /// <param name="tableNames">同步指定表，多个“,”分割</param>
        /// <returns></returns>
        public ApiResult DBMigration(IFreeSql fsql, string assemblies, string syncNameSpace, string tableNames = "")
        {
            if (string.IsNullOrWhiteSpace(assemblies) || string.IsNullOrWhiteSpace(syncNameSpace)) return ApiResult.Error();
            var assembliesArr = assemblies.SplitWithSemicolon();
            var syncNameSpaceArr = syncNameSpace.SplitWithSemicolon();
            if (assembliesArr.Length == 0 || syncNameSpaceArr.Length == 0) return ApiResult.Error();

            var tableNameList = tableNames.SplitWithComma().ToList();

            List<Type> initTypes = new();
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
                                if (tableNameList.Contains(item.Name))
                                {
                                    initTypes.Add(item);
                                    _logger.LogInformation($"迁移实体：{item.FullName}");
                                }
                            }
                            else
                            {
                                initTypes.Add(item);
                                _logger.LogInformation($"迁移实体：{item.FullName}");
                            }
                        }
                    }
                }
            }

            fsql.CodeFirst.SyncStructure(initTypes.ToArray());

            return ApiResult.OK();
        }

        /// <summary>
        /// 同步视图文件
        /// </summary>
        /// <param name="fsql"></param>
        /// <param name="folder">视图所在根目录</param>
        /// <returns></returns>
        public ApiResult SqlViewSync(IFreeSql fsql, string folder)
        {
            //刷新视图
            string sql = "select TABLE_NAME from INFORMATION_SCHEMA.VIEWS order by TABLE_NAME";
            var viewList = fsql.Ado.Query<string>(sql);
            foreach (var viewName in viewList)
            {
                sql = String.Format("exec sp_refreshview {0}", viewName);
                fsql.Ado.ExecuteNonQuery(sql);
                _logger.LogInformation($"刷新视图：{viewName}");
            }

            //更新视图
            if (!Directory.Exists(folder)) return ApiResult.OK("视图根目录不存在，此次仅刷新已有的视图");

            var sqlFileList = Directory.GetFiles(folder, "*.sql", SearchOption.AllDirectories);
            foreach (var sqlFilePath in sqlFileList)
            {
                var sqlViewText = File.ReadAllText(sqlFilePath);
                if (sqlViewText.IsNull()) continue;

                var arrSqlView = sqlViewText.Split(new string[2]{
                                        "\r\ngo\r\n",
                                        "\r\nGO\r\n"
                                    }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var sqlItem in arrSqlView)
                {
                    if (string.IsNullOrEmpty(sqlItem)) continue;
                    string execSql = sqlItem;

                    try
                    {
                        if (sqlItem.EndsWith("go") || sqlItem.EndsWith("GO"))
                        {
                            execSql = sqlItem.Substring(0, sqlItem.Length - 2);
                        }

                        fsql.Ado.ExecuteNonQuery(execSql);
                    }

                    catch (Exception e)
                    {
                        _logger.LogError($"执行ViewSql文件出错，文件：{sqlFilePath}，错误信息：{e.Message}");
                        continue;
                    }
                }
                _logger.LogInformation($"更新视图数量：{arrSqlView.Length}");
            }

            return ApiResult.OK();

        }

    }
}
