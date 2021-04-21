/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：系统用户组仓储实现                                                    
*│　作    者：litdev                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2021-03-04 09:04:26                            
*└──────────────────────────────────────────────────────────────┘
*/

using FreeSql;
using MyNetCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 系统用户组仓储实现
    /// </summary>
    public class SysRoleRepository : BaseMyRepository<SysRole, int>, ISysRoleRepository
    {

        public SysRoleRepository(ILogger<SysRoleRepository> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {

        }

        /// <summary>
        /// 获取用户组的权限信息
        /// </summary>
        /// <param name="roleId">用户组id</param>
        /// <returns></returns>
        public async Task<List<Model.Dto.SysRoleModuleGroupModel>> GetPermitListByRoleId(int roleId)
        {
            var sql = string.Format(@"select sm.ModuleName, sh.HandlerName, sp.PermitId,sp.PermitName, sp.AliasName, sh.OrderNo,
                                        case when ISNULL(srp.PermitId,0)!=0  then 'true' else 'false' end IsChecked
                                        from SysPermit as sp
                                        inner join SysHandler as sh on sp.HandlerId = sh.HandlerId
                                        inner join SysModule as sm on sh.ModuleId = sm.ModuleId
                                        left join SysRolePermit as srp on srp.PermitId = sp.PermitId and srp.RoleId = {0}",
                                        roleId);
            var dbData = await _fsql.Select<Model.Dto.SysRolePermit>().WithSql(sql).ToListAsync();

            var moduleGroup = dbData.GroupBy(p => p.ModuleName).ToList();

            var resultList = new List<Model.Dto.SysRoleModuleGroupModel>();

            foreach (var module in moduleGroup)
            {
                var moduleModel = new Model.Dto.SysRoleModuleGroupModel(module.Key);

                var handlerGroup = dbData.Where(o => o.ModuleName == module.Key).ToList().GroupBy(o => o.HandlerName).ToList();

                foreach (var handler in handlerGroup)
                {
                    var handlerModel = new Model.Dto.SysRoleHandlerGroupModel(handler.Key);

                    handlerModel.PermitList = handler.OrderByDescending(p => p.OrderNo).ToList();

                    moduleModel.HandlerList.Add(handlerModel);
                }
                resultList.Add(moduleModel);
            }

            return resultList;

        }

        /// <summary>
        /// 设置用户组权限
        /// </summary>
        /// <param name="roleId">组id</param>
        /// <param name="permits">权限id组</param>
        /// <returns></returns>
        public Task<bool> SetRolePermit(int roleId, string permits)
        {
            var rolePermitList = new List<SysRolePermit>();
            foreach (var item in permits.SplitWithComma())
            {
                rolePermitList.Add(new SysRolePermit()
                {
                    PermitId = item.ObjToInt(),
                    RoleId = roleId,
                });
            }
            _fsql.Transaction(() =>
            {
                _fsql.Delete<SysRolePermit>().Where(p => p.RoleId == roleId).ExecuteAffrowsAsync();
                _fsql.Insert(rolePermitList).ExecuteAffrowsAsync();
            });
            return Task.FromResult(true);
        }

        /// <summary>
        /// 根据用户id获取所属的用户组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<SysRole>> GetRoleListByUserId(int userId)
        {
            string sql = string.Format(@"select b.* from SysRoleUser as a inner join SysRole as b on a.RoleId = b.RoleId and a.UserId = {0} and b.IsDeleted = 0 and b.[Status] = 1", userId);
            return _fsql.Select<SysRole>().WithSql(sql).ToListAsync();
        }

        /// <summary>
        /// 根据组id获取所拥有的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<List<string>> GetPermissionsByRoleIds(string roleIds)
        {
            string sql = string.Format(@"select DISTINCT (c.AliasName + '.' + b.AliasName) as auth from 
                                            SysRolePermit as a left join SysPermit as b on a.PermitId = b.PermitId and a.IsDeleted = 0 and a.RoleId in({0}) 
                                            left join SysHandler as c on b.HandlerId = c.HandlerId and c.IsDeleted=0", roleIds);

            return _fsql.Ado.QueryAsync<string>(sql);
        }

    }
}
