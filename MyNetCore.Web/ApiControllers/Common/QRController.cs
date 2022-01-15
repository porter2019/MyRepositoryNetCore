namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 二维码生成
    /// </summary>
    [HiddenApi]
    public class QRController : BaseOpenApiController
    {
        private readonly ILogger<QRController> _logger;

        public QRController(ILogger<QRController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="_qr"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public async Task<FileContentResult> Get([FromServices] IQRCodeService _qr, string content)
        {
            //var buffer = await _qr.GenerateQRCode(content);
            var iconPath = _hostEnvironment.ContentRootPath;//D:\\WorkSpace\\GitHub\\MyNetCore\\MyNetCore.Web
            iconPath += "\\wwwroot\\favicon.ico";
            if (System.IO.File.Exists(iconPath))
            {
                var buffer = await _qr.GenerateQRCode(content, iconPath);
                return File(buffer, "image/jpeg");
            }
            else
            {
                var buffer = await _qr.GenerateQRCode(content);
                return File(buffer, "image/jpeg");
            }
        }
    }
}