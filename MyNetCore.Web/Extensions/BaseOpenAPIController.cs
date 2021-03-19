using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web
{
    /// <summary>
    /// API控制器基类
    /// </summary>
    [TypeFilter(typeof(OpenApiActionFilterAttribute))]
    [Route("api/open/[controller]")]
    public class BaseOpenApiController : BaseApiController
    {

        /// <summary>
        /// 上下文Cookie中的用户信息
        /// <code>数据在OpenApiActionFilterAttribute中填充</code>
        /// </summary>
        public CurrentUserTickInfo CurrentUserInfo;

        public BaseOpenApiController()
        {
            
        }

        /// <summary>
        /// 手动校验权限
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="action">操作标识，多个逗号分割</param>
        /// <returns></returns>
        protected bool CheckPermission(Type controllerType, string action)
        {
            //if (action.IsNull()) return true;
            //var operations = action.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //var entityPrivHandler = _dbEC.Select<PrivHandler>().Where(p => p.RefController == controllerType.FullName).ToOne();
            //if (entityPrivHandler == null) return false;
            //string operationSql = $"select distinct PermitName from PrivUserPermit as A left join PrivPermit as B on A.PermitId = B.PermitId where A.UserId = {CurrentCookieUserInfo.UserId} and B.HandlerId = {entityPrivHandler.HandlerId}";
            //var userHaveOperationList = _dbEC.Ado.Query<string>(operationSql);
            //if (userHaveOperationList.Count == 0) return false;
            //var intersectLs = operations.Intersect(userHaveOperationList);
            //if (intersectLs.Count() == 0) return false;
            return true;
        }

    }
}
