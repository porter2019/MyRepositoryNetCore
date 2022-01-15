namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 验证码演示
    /// </summary>
    public class CaptchaController : BaseOpenApiController
    {
        private readonly ILogger<CaptchaController> _logger;

        public CaptchaController(ILogger<CaptchaController> logger)
        {
            _logger = logger;
        }

        [HttpGet, Route("get"), HiddenApi]
        public async Task<FileContentResult> CaptchaAsync([FromServices] ICaptchaService _captchaServices)
        {
            //if (_captchaServices == null) throw new ArgumentNullException(nameof(_captchaServices), "ICaptchaServices未注入，请在CaptchaServices的属性ServiceLifetimeAttribute中启用");
            var code = await _captchaServices.GenerateRandomCaptchaAsync();
            var result = await _captchaServices.GenerateCaptchaImageAsync(code);

            return File(result.CaptchaMemoryStream.ToArray(), "image/png");
        }
    }
}