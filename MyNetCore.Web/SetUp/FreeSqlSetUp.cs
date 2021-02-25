using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyNetCore.Web.SetUp
{
    public static class FreeSqlSetUp
    {
        public static void AddFreeSqlSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var logger = NLog.LogManager.GetCurrentClassLogger();

            var sqlTypeConfig = AppSettings.Get("DBContexts", "DBType");
            var sqlType = (global::FreeSql.DataType)Enum.Parse(typeof(global::FreeSql.DataType), sqlTypeConfig);
            var connectionString = AppSettings.Get("DBContexts", sqlTypeConfig, "ConnectionString");
            var isAutoMigration = AppSettings.Get<bool>("DBContexts", sqlTypeConfig, "IsAutoMigration");
            var lazyLoading = AppSettings.Get<bool>("DBContexts", sqlTypeConfig, "LazyLoading");

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
                .Build();

            freeSql.Aop.CurdAfter += (s, e) =>
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                foreach (var item in e.DbParms)
                {
                    if (item == null) continue;
                    stringBuilder.Append($"{item.ParameterName}-{ item.Value},");
                }

                var logTxt = $"【{e.CurdType}】实体：{e.EntityType.FullName},耗时：{e.ElapsedMilliseconds},执行的SQL：{e.Sql}";
                if (stringBuilder.Length > 0)
                    logTxt += "，参数：" + stringBuilder.ToString();
                logTxt += "，返回值：" + e.ExecuteResult.ToString();
                logger.Info(logTxt);
            };

            //必须定义为单例模式
            services.AddSingleton(freeSql);

        }
    }
}
