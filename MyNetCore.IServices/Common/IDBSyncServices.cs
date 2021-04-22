using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 数据库同步相关服务
    /// </summary>
    public interface IDBSyncServices : IBatchDIServicesTag
    {
        /// <summary>
        /// 数据库CodeFrist迁移
        /// </summary>
        /// <param name="fsql"></param>
        /// <param name="assemblies">实体程序集名称</param>
        /// <param name="syncNameSpace">同步指定命名空间开头的实体类</param>
        /// <param name="tableNames">同步指定表，多个“,”分割</param>
        /// <returns></returns>
        ApiResult DBMigration(IFreeSql fsql, string assemblies, string syncNameSpace, string tableNames = "");

        /// <summary>
        /// 同步视图文件
        /// </summary>
        /// <param name="fsql"></param>
        /// <param name="folder">视图所在根目录</param>
        /// <returns></returns>
        ApiResult SqlViewSync(IFreeSql fsql, string folder);

    }
}
