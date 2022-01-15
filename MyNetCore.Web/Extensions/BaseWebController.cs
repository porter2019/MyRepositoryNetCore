using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyNetCore.Web
{
    /// <summary>
    /// MVC控制器基类
    /// </summary>
    public class BaseWebController : Controller
    {
        protected readonly IHostEnvironment _hostEnvironment;
        protected readonly IModelMetadataProvider _modelMetadataProvider;

        public BaseWebController()
        {
            var hca = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
            _hostEnvironment = hca.HttpContext.RequestServices.GetService<IHostEnvironment>();
            _modelMetadataProvider = hca.HttpContext.RequestServices.GetService<IModelMetadataProvider>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var controller = context.Controller as Controller;

            //获取Cookie中的用户信息，并添加到上下文

            //controller.ViewBag.UserIsLogin = true;
        }

        #region Session

        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetSessionString(string key)
        {
            return HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected int GetSessionInt(string key)
        {
            return HttpContext.Session.GetInt32(key).GetValueOrDefault(0);
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetSession(string key, int value)
        {
            HttpContext.Session.SetInt32(key, value);
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveSession(string key)
        {
            HttpContext.Session.Remove(key);
        }

        #endregion Session

        #region Cookie

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetCookies(string key, string value)
        {
            HttpContext.Response.Cookies.Append(key, value);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time">过期时间，单位：分钟</param>
        protected void SetCookies(string key, string value, int time)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(time)
            });
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        protected void SetCookies(string key, string value, string domain)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions { Domain = domain, Path = "/" });
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain">指定域名,domain.com</param>
        /// <param name="time">过期时间，单位：分钟</param>
        protected void SetCookies(string key, string value, string domain, int time)
        {
            //设置HttpOnly=true的cookie不能被js获取到
            //Secure=true，那么这个cookie只能用https协议发送给服务器，用http协议是不发送的
            if (time <= 0) SetCookies(key, value, domain);
            else HttpContext.Response.Cookies.Append(key, value, new CookieOptions { Domain = domain, Path = "/", Expires = DateTime.Now.AddMinutes(time) });
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetCookie(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            return value;
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveCookie(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="domain"></param>
        protected void RemoveCookie(string key, string domain)
        {
            HttpContext.Response.Cookies.Delete(key, new CookieOptions() { Domain = domain, Path = "/" });
        }

        #endregion Cookie

        //#region 统一Post控制器返回的View

        ///// <summary>
        ///// 返回Post控制器结果View
        ///// </summary>
        ///// <param name="webResult"></param>
        ///// <param name="postModel"></param>
        ///// <returns></returns>
        //protected IActionResult View(ApiResult webResult, object postModel)
        //{
        //    if (webResult.code == ApiCode.成功)
        //    {
        //        return PromptView("Index", "保存成功");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("TempId", webResult.msg);
        //        return View("Edit", postModel);
        //    }
        //}

        ///// <summary>
        ///// 返回验证失败的Edit页面
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //protected ViewResult VerifyErrorView(object postModel)
        //{
        //    return View("Edit", postModel);
        //}

        ///// <summary>
        ///// 添加模型验证错误消息
        ///// </summary>
        ///// <param name="message"></param>
        //protected void AddModelValidateError(string message)
        //{
        //    ModelState.AddModelError("TempId", message);
        //}

        //#endregion
    }
}