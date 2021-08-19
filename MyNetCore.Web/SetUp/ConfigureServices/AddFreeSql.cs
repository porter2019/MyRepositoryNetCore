using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace MyNetCore.Web.SetUp
{
    public static class AddFreeSql
    {
        /// <summary>
        /// 注入FreeSql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddFreeSqlServices(this IServiceCollection services, IConfiguration config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var logger = NLog.LogManager.GetCurrentClassLogger();

            var appName = AppDomain.CurrentDomain.FriendlyName;

            string logTemplate = "【" + appName + "】数据库：{0}，类别：{1}，实体：{2}，耗时：{3}毫秒，SQL：{4}";

            #region 注入主数据库

            {
                var pathRoot = "DBContexts:Main";
                var sqlTypeConfig = config[$"{pathRoot}:DBType"];
                var sqlType = (global::FreeSql.DataType)Enum.Parse(typeof(global::FreeSql.DataType), sqlTypeConfig);
                var connectionString = config[$"{pathRoot}:{sqlTypeConfig}:ConnectionString"];
                var isAutoMigration = config.GetValue<bool>($"{pathRoot}:{sqlTypeConfig}:IsAutoMigration");
                var lazyLoading = config.GetValue<bool>($"{pathRoot}:{sqlTypeConfig}:LazyLoading");

                var freeSql = new FreeSql.FreeSqlBuilder()
                    .UseConnectionString(sqlType, connectionString)
                    .UseAutoSyncStructure(isAutoMigration)
                    .UseLazyLoading(lazyLoading)
                    .UseMonitorCommand(
                        cmd =>
                        {
                            //监听SQL命令对象，在执行前
                            //logger.Info("执行前："+cmd.CommandText);
                        },
                        (cmd, traceLog) =>
                        {
                            //监听SQL命令对象，在执行后
                            logger.Debug($"【{appName}】Main数据库执行后：{traceLog}");
                        })
                    .Build<DBFlagMain>();

                freeSql.Aop.CurdAfter += (s, e) =>
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    foreach (var item in e.DbParms)
                    {
                        if (item == null) continue;
                        stringBuilder.Append($"[{item.ParameterName}] = [{ item.Value}],");
                    }

                    var logTxt = string.Format(logTemplate, "Main", e.CurdType, e.EntityType.FullName, e.ElapsedMilliseconds, e.Sql);
                    if (stringBuilder.Length > 0)
                        logTxt += "，参数：" + stringBuilder.ToString();
                    logTxt += "，返回值：" + JsonConvert.SerializeObject(e.ExecuteResult);
                    logger.Info(logTxt);
                };
                freeSql.GlobalFilter.Apply<ISoftDelete>("SoftDelete", a => a.IsDeleted == false);

                //必须定义为单例模式
                services.AddSingleton(freeSql);

            }

            #endregion


            #region 注入次数据库

            {
                var pathRoot = "DBContexts:Secondary";
                var sqlTypeConfig = config[$"{pathRoot}:DBType"];
                var sqlType = (global::FreeSql.DataType)Enum.Parse(typeof(global::FreeSql.DataType), sqlTypeConfig);
                var connectionString = config[$"{pathRoot}:{sqlTypeConfig}:ConnectionString"];
                var isAutoMigration = config.GetValue<bool>($"{pathRoot}:{sqlTypeConfig}:IsAutoMigration");
                var lazyLoading = config.GetValue<bool>($"{pathRoot}:{sqlTypeConfig}:LazyLoading");

                var freeSql = new FreeSql.FreeSqlBuilder()
                    .UseConnectionString(sqlType, connectionString)
                    .UseAutoSyncStructure(isAutoMigration)
                    .UseLazyLoading(lazyLoading)
                    .UseMonitorCommand(
                        cmd =>
                        {
                            //监听SQL命令对象，在执行前
                            //logger.Info("执行前："+cmd.CommandText);
                        },
                        (cmd, traceLog) =>
                        {
                            //监听SQL命令对象，在执行后
                            logger.Debug($"【{appName}】Secondary数据库执行后：{traceLog}");
                        })
                    .Build<DBFlagSecondary>();

                freeSql.Aop.CurdAfter += (s, e) =>
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    foreach (var item in e.DbParms)
                    {
                        if (item == null) continue;
                        stringBuilder.Append($"[{item.ParameterName}] = [{ item.Value}],");
                    }

                    var logTxt = string.Format(logTemplate, "Main", e.CurdType, e.EntityType.FullName, e.ElapsedMilliseconds, e.Sql);
                    if (stringBuilder.Length > 0)
                        logTxt += "，参数：" + stringBuilder.ToString();
                    logTxt += "，返回值：" + JsonConvert.SerializeObject(e.ExecuteResult);
                    logger.Info(logTxt);
                };
                freeSql.GlobalFilter.Apply<ISoftDelete>("SoftDelete", a => a.IsDeleted == false);

                //必须定义为单例模式
                services.AddSingleton(freeSql);

            }

            #endregion

            var repositoryAssmbly = System.Reflection.Assembly.Load($"{services.GetProjectMainName()}.Repository");
            //注入仓储
            services.AddFreeRepository(null, repositoryAssmbly);
        }

    }

    /// <summary>
    /// FreeSql软删除标记
    /// </summary>
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }


}
