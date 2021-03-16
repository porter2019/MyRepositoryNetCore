using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyNetCore.IServices;
using MyNetCore.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web.Controllers
{
    public class HomeController : BaseWebController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var projectName = AppDomain.CurrentDomain.FriendlyName;

            return View();
        }

        public async Task<ContentResult> Test([FromServices] IViewRenderService _iView)
        {
            var model = new Model.Entity.SysUser()
            {
                UserName = "ABC",
                LoginName = "EDFD"
            };
            var html = await _iView.RenderViewToStringAsync($"/Views/CodeGenerateTemplate/Test.cshtml", model);
            return Content(html);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
