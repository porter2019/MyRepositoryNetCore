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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDemoServices _demoServices;

        public HomeController(ILogger<HomeController> logger, IDemoServices demoServices)
        {
            _logger = logger;
            _demoServices = demoServices;
        }

        public IActionResult Index()
        {
            //测试调用服务代码
            var count = _demoServices.Sum(1, 2);

            var projectName = AppDomain.CurrentDomain.FriendlyName;


            return View();
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
