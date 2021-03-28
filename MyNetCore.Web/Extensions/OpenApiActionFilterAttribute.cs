using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNetCore.IServices;

namespace MyNetCore.Web
{
    /// <summary>
    /// 接口Action过滤器
    /// </summary>
    public class OpenApiActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<OpenApiActionFilterAttribute> _logger;
        private readonly ISysUserServices _sysUserServices;

        public OpenApiActionFilterAttribute(IHostEnvironment hostEnvironment, ILogger<OpenApiActionFilterAttribute> logger, ISysUserServices sysUserServices)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _sysUserServices = sysUserServices;
        }

        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            #region 是否是API控制器

            var isApiController = context.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            if (!isApiController) return;
            var controllerActionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;

            #endregion

            #region 是否可以匿名访问

            //var anonymous = false;

            var permissionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Where(p => p.GetType().Equals(typeof(PermissionAttribute))).FirstOrDefault() as PermissionAttribute;
            if (permissionAttributes != null)
            {
                //anonymous = permissionAttributes.Anonymous;
                if (permissionAttributes.Anonymous) return;
            }

            //if (anonymous) return;

            #endregion

            #region 将用户信息附加到上下文中

            var controller = context.Controller as BaseOpenApiController;
            //获取JWT中用户的信息
            //controller.CurrentUserInfo = new CurrentUserTickInfo() { UserId = 2, UserName = "管理员", LoginName = "admin", ExpireTime = DateTime.Now.AddDays(3) };
            var tokenValue = context.HttpContext.Request.Headers[GlobalVar.AuthenticationTokenKey].FirstOrDefault()?.ToString() ?? "";
            if(tokenValue.IsNull())
            {
                context.Result = new ObjectResult(ApiResult.Anonymous());
                return;
            }

            var loginName = tokenValue.Split("-")[0];
            var userEntity = _sysUserServices.GetModel(p => p.LoginName == loginName);
            if(userEntity == null)
            {
                context.Result = new ObjectResult(ApiResult.Anonymous());
                return;
            }
            if (!userEntity.Status)
            {
                context.Result = new ObjectResult(ApiResult.Anonymous());
                return;
            }
            controller.CurrentUserInfo = new CurrentUserTickInfo() { UserId = userEntity.Id, UserName = userEntity.UserName, LoginName = userEntity.LoginName, ExpireTime = DateTime.Now.AddDays(3) };

            if (controller.CurrentUserInfo == null)
            {
                context.Result = new ObjectResult(ApiResult.Anonymous());
                return;
            }

            #endregion

            #region 验证权限

            //功能控制器名称
            var refController = context.Controller.GetType().FullName;
            //先不验证权限了，针对审核问题
            //OpenAPI接口权限验证，子账号才验证权限
            //if (permissionAttributes != null && controller.CurrentCookieUserInfo.UserType == 1)
            //{
            //    if (permissionAttributes.AutoCheck && !string.IsNullOrWhiteSpace(permissionAttributes.OperationName))
            //    {
            //        //操作
            //        var operations = permissionAttributes.OperationName.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //        //权限
            //        var entityPrivHandler = _dbEC.Select<PrivHandler>().Where(p => p.RefController == refController).ToOne();
            //        if (entityPrivHandler == null)
            //        {
            //            context.Result = new ObjectResult(ApiResult.Error("权限Handler没有注册"));
            //            return;
            //        }
            //        string operationSql = $"select distinct PermitName from PrivUserPermit as A left join PrivPermit as B on A.PermitId = B.PermitId where A.UserId = {controller.CurrentCookieUserInfo.UserId} and B.HandlerId = {entityPrivHandler.HandlerId}";
            //        var userHaveOperationList = _dbEC.Ado.Query<string>(operationSql);
            //        if (userHaveOperationList.Count == 0)
            //        {
            //            context.Result = new ObjectResult(ApiResult.Forbidden());
            //            return;
            //        }
            //        var intersectLs = operations.Intersect(userHaveOperationList);
            //        if (intersectLs.Count() == 0)
            //        {
            //            context.Result = new ObjectResult(ApiResult.Forbidden());
            //            return;
            //        }
            //    }
            //}

            #endregion

            #region 自动填充请求实体中的用户信息

            object postModelValue = context.ActionArguments.FirstOrDefault().Value;

            if ((postModelValue as IEnumerable<object>) != null)
                postModelValue = (postModelValue as IEnumerable<object>).ToList()[0];

            //数据库实体对象
            if (postModelValue is Model.BaseEntityStandard && context.HttpContext.Request.Method == "POST")
            {
                Model.BaseEntityStandard crudModel = (Model.BaseEntityStandard)postModelValue;

                if (crudModel.Id < 1)
                {
                    crudModel.CreatedDate = DateTime.Now;
                    crudModel.CreatedUserId = controller.CurrentUserInfo.UserId;
                    crudModel.CreatedUserName = controller.CurrentUserInfo.UserName;
                }
            }
            //ViewMode对象
            if (postModelValue is Model.BaseRequestViewModel && context.HttpContext.Request.Method == "POST")
            {
                Model.BaseRequestViewModel reqModel = (Model.BaseRequestViewModel)postModelValue;
                reqModel.CurrentUserId = controller.CurrentUserInfo.UserId;
                reqModel.CurrentUserName = controller.CurrentUserInfo.UserName;
            }

            #endregion

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

    }
}
