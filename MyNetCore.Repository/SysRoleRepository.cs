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

namespace MyNetCore.Repository
{
    /// <summary>
    /// 系统用户组仓储实现
    /// </summary>
    public class SysRoleRepository : BaseMyRepository<SysRole, int>, ISysRoleRepository
    {
        private readonly IFreeSql _freeSql;

        public SysRoleRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
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
            var dbData = await _freeSql.Select<Model.Dto.SysRolePermit>().WithSql(sql).ToListAsync();

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
    }
}
