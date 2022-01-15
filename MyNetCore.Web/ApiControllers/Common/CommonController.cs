namespace MyNetCore.Web.ApiControllers
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController : BaseOpenApiController
    {
        private readonly ILogger<CommonController> _logger;
        private readonly ISMSService _SMSServices;
        private readonly IAttachUploadService _attachUploadService;

        public CommonController(ILogger<CommonController> logger,
            IAttachUploadService attachUploadService,
            ISMSService SMSServices)
        {
            _logger = logger;
            _SMSServices = SMSServices;
            _attachUploadService = attachUploadService;
        }

        #region 文件上传

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("upload")]
        public ApiResult FileUpload()
        {
            var saveResult = _attachUploadService.SaveAttach(HttpContext.Request.Form.Files, HttpContext.Request.Form["tag"].ToString());
            return ApiResult.OK(saveResult);
        }

        #endregion 文件上传

        #region 验证码相关

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        [HttpGet, Route("vcode/send")]
        public async Task<ApiResult> SendValidateCode(int type, string mobile)
        {
            string guid = Guid.NewGuid().ToString("N");
            string code = Common.Helper.RandomHelper.GenerateIntNumber(6);

            var data = await _SMSServices.SendTestAsync(guid, mobile, code);
            if (data.code == ApiCode.成功)
            {
                data.data = guid;
            }

            return data;
        }

        /// <summary>
        /// 校验验证码是否正确
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet, Route("vcode/check")]
        public Task<ApiResult> CheckValidateCode(string guid, string code)
        {
            return _SMSServices.ValidateCodeAsync(guid, code);
        }

        #endregion 验证码相关
    }
}