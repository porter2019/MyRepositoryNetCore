using Microsoft.Extensions.DependencyInjection;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyNetCore.Web.SetUp
{
    public static class AddFreeSql
    {
        /// <summary>
        /// 注入FreeSql
        /// </summary>
        /// <param name="services"></param>
        public static void AddFreeSqlServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var logger = NLog.LogManager.GetCurrentClassLogger();

            #region 注入主数据库

            {
                var pathRoot = "DBContexts:Main";
                var sqlTypeConfig = AppSettings.Get($"{pathRoot}:DBType");
                var sqlType = (global::FreeSql.DataType)Enum.Parse(typeof(global::FreeSql.DataType), sqlTypeConfig);
                var connectionString = AppSettings.Get($"{pathRoot}:{sqlTypeConfig}:ConnectionString");
                var isAutoMigration = AppSettings.Get<bool>($"{pathRoot}:{sqlTypeConfig}:IsAutoMigration");
                var lazyLoading = AppSettings.Get<bool>($"{pathRoot}:{sqlTypeConfig}:LazyLoading");

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
                            logger.Debug($"执行后：{traceLog}");
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

                    var logTxt = $"【{e.CurdType}】实体：{e.EntityType.FullName},耗时：{e.ElapsedMilliseconds}毫秒,执行的SQL：{e.Sql}";
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
                var sqlTypeConfig = AppSettings.Get($"{pathRoot}:DBType");
                var sqlType = (global::FreeSql.DataType)Enum.Parse(typeof(global::FreeSql.DataType), sqlTypeConfig);
                var connectionString = AppSettings.Get($"{pathRoot}:{sqlTypeConfig}:ConnectionString");
                var isAutoMigration = AppSettings.Get<bool>($"{pathRoot}:{sqlTypeConfig}:IsAutoMigration");
                var lazyLoading = AppSettings.Get<bool>($"{pathRoot}:{sqlTypeConfig}:LazyLoading");

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
                            logger.Debug($"执行后：{traceLog}");
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

                    var logTxt = $"【{e.CurdType}】实体：{e.EntityType.FullName},耗时：{e.ElapsedMilliseconds}毫秒,执行的SQL：{e.Sql}";
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
