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
            return Content(_hostEnvironment.EnvironmentName);
        }

    }
}
