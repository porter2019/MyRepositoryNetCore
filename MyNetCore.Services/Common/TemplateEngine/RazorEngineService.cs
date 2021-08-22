using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using MyNetCore.IServices;

namespace MyNetCore.Services
{
    /// <summary>
    /// 使用RazorEngines生成代码
    /// 注意Startup中要使用MVC
    /// </summary>
    [ServiceLifetime(false)]
    public class RazorEngineService : ITemplateEngineService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IHttpContextAccessor _contextAccessor;


        public RazorEngineService(IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider, IHttpContextAccessor contextAccessor)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _contextAccessor = contextAccessor;

        }

        /// <summary>
        /// 根据模板解析成最终的字符串
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ParseAsync(string viewName, object model)
        {
            var actionContext = new ActionContext(_contextAccessor.HttpContext, _contextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = FindView(actionContext, viewName);

                if (viewResult == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 查找视图文件
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _razorViewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _razorViewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }
    }
}
