﻿using MyNetCore.Common.Helper;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace MyNetCore.Services
{
    /// <summary>
    /// 发送短信服务
    /// </summary>
    [ServiceLifetime(true)]
    public class AliSMSService : ISMSService
    {
        private readonly ICacheService _cacheServices;
        private readonly IValidateCodeHistoryService _validateCodeHistoryServices;
        private readonly ILogger _logger;
        private readonly IHttpService _httpService;
        private readonly IConfiguration _config;
        private readonly bool _apiEnabled;

        public AliSMSService(ICacheService cacheServices, 
            IValidateCodeHistoryService validateCodeHistoryServices, 
            ILogger<AliSMSService> logger, 
            IHttpService httpService,
            IConfiguration config)
        {
            _logger = logger;
            _httpService = httpService;
            _cacheServices = cacheServices;
            _validateCodeHistoryServices = validateCodeHistoryServices;
            _logger = logger;
            _config = config;
            _apiEnabled = _config.GetValue<bool>("SMSAli:IsEnabled");
        }

        /// <summary>
        /// 发送测试验证码
        /// </summary>
        /// <param name="guid">唯一标识</param>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ApiResult> SendTestAsync(string guid, string mobile, string code)
        {
            if (await CheckSendIsOftenAsync(mobile)) return ApiResult.Failed("发送太频繁了");

            var jsonData = JsonHelper.Serialize(new { code = code });

            var sendResult = await SendAsync(mobile, jsonData, "SMS_128965131");

            return await SaveSendHistoryAsync(sendResult, jsonData, guid, mobile, code);
        }

        /// <summary>
        /// 校验验证码是否有效
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ApiResult> ValidateCodeAsync(string guid, string code)
        {
            var cacheValue = await _cacheServices.GetAsync(guid);
            if (cacheValue.IsNull()) return ApiResult.Failed("验证码无效");

            if (!cacheValue.Equals(code))
                return ApiResult.Failed("验证码错误");
            else
            {
                await _cacheServices.RemoveAsync(guid);
                return ApiResult.OK();
            }
        }

        #region 私有方法

        /// <summary>
        /// 校验验证码是否发送频繁
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private async Task<bool> CheckSendIsOftenAsync(string mobile)
        {
            var sendTime = await _cacheServices.GetAsync(mobile);
            if (sendTime.IsNull()) return false;
            _logger.LogDebug("缓存中的:" + sendTime);
            if ((DateTime.Now - sendTime.ObjToInt().ConvertToDateTime()).TotalSeconds <= 60)
                return true;//60秒内的为发送频繁
            else
                return false;
        }

        /// <summary>
        /// 验证码发送记录入库
        /// </summary>
        /// <param name="sendResult">验证码发送结果</param>
        /// <param name="sendBody">发送的内容</param>
        /// <param name="guid">唯一标识</param>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        private async Task<ApiResult> SaveSendHistoryAsync(ApiResult sendResult, string sendBody, string guid, string mobile, string code)
        {
            if (sendResult.code != ApiCode.成功) return ApiResult.Error(sendResult.msg);

            //用于校验验证码是否正确
            await _cacheServices.AddAsync(guid, code, 60 * 10);
            //用于校验是否发送频繁
            await _cacheServices.AddAsync(mobile, DateTime.Now.ToTimeStamp(), 60);
            _logger.LogDebug($"【发送验证码】内容：{sendBody}，GUID：{guid}，手机号：{mobile}，验证码：{code}，返回结果：{JsonHelper.Serialize(sendResult)}");
            //保存到数据库中
            await _validateCodeHistoryServices.InsertAsync(new Model.Entity.ValidateCodeHistory()
            {
                Address = mobile,
                Body = sendBody,
                IsSendOK = sendResult.code == ApiCode.成功,
                Result = JsonHelper.Serialize(sendResult),
                UUID = guid,
                Type = Model.Entity.SMSType.CellPhone,
            });

            return ApiResult.OK();
        }

        #endregion 私有方法

        #region 调用发送 API

        /// <summary>
        /// 阿里云发送短信验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="jsonData">Json数据</param>
        /// <param name="templateCode">templateCode</param>
        /// <returns></returns>
        private async Task<ApiResult> SendAsync(string mobile, string jsonData, string templateCode)
        {
            if (!ValidateHelper.IsCellPhone(mobile)) return ApiResult.ValidateFail();

            if (!_apiEnabled) return ApiResult.Error("管理员禁用短信发送Api");

            string EndPoint = _config["SMSAli:EndPoint"];
            string AccessKeyId = _config["SMSAli:AccessKeyId"];
            string AccessKeySecret = _config["SMSAli:AccessKeySecret"];
            string SignName = _config["SMSAli:SignName"];

            if (SignName.IsNull()) return ApiResult.Error("管理员未配置短信发送相关参数");

            string nowDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");//GTM时间
            Dictionary<string, string> keyValues = new Dictionary<string, string>();//声明一个字典
            //1.系统参数
            keyValues.Add("SignatureMethod", "HMAC-SHA1");
            keyValues.Add("SignatureNonce", Guid.NewGuid().ToString());
            keyValues.Add("AccessKeyId", AccessKeyId);
            keyValues.Add("SignatureVersion", "1.0");
            keyValues.Add("Timestamp", nowDate);
            keyValues.Add("Format", "Json");//可换成xml

            //2.业务api参数
            keyValues.Add("Action", "SendSms");
            keyValues.Add("Version", "2017-05-25");
            keyValues.Add("RegionId", "cn-hangzhou");
            keyValues.Add("PhoneNumbers", mobile);
            keyValues.Add("SignName", SignName);
            keyValues.Add("TemplateParam", jsonData);
            keyValues.Add("TemplateCode", templateCode);
            keyValues.Add("OutId", "");

            //3.去除签名关键字key
            if (keyValues.ContainsKey("Signature"))
            {
                keyValues.Remove("Signature");
            }

            //4.参数key排序
            Dictionary<string, string> ascDic = keyValues.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value.ObjToString());
            //System.Diagnostics.Trace.WriteLine(">>>>>>>>>>>>>>>>>>"+JsonHelper.Serialize(ascDic));
            //5.构造待签名的字符串
            StringBuilder builder = new StringBuilder();
            foreach (var item in ascDic)
            {
                if (item.Key == "SignName")
                {
                }
                else
                {
                    builder.Append('&').Append(specialUrlEncode(item.Key)).Append('=').Append(specialUrlEncode(item.Value));
                }
                if (item.Key == "RegionId")
                {
                    builder.Append('&').Append(specialUrlEncode("SignName")).Append('=').Append(specialUrlEncode(keyValues["SignName"]));
                }
            }
            string sorteQueryString = builder.ToString().Substring(1);

            StringBuilder stringToSign = new StringBuilder();
            stringToSign.Append("GET").Append('&');
            stringToSign.Append(specialUrlEncode("/")).Append('&');
            stringToSign.Append(specialUrlEncode(sorteQueryString));

            string Sign = MySign(AccessKeySecret + "&", stringToSign.ToString());
            //6.签名最后也要做特殊URL编码
            string signture = specialUrlEncode(Sign);

            //最终打印出合法GET请求的URL
            string url = string.Format("http://{0}/?Signature={1}{2}", EndPoint, signture, builder);
            var result = await _httpService.GetAsync(url);
            if (result.IsNull()) _logger.LogError($"【阿里云SMS】手机号:{mobile}，模板内容：{jsonData}，返回结果为空");
            _logger.LogInformation($"【阿里云SMS】手机号:{mobile}，模板内容：{jsonData}，返回结果：{result}");
            if (result == "OK") return ApiResult.OK("OK", jsonData);
            else return ApiResult.ValidateFail(result, jsonData);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string specialUrlEncode(string s)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                string t = s[i].ToString();
                string k = System.Web.HttpUtility.UrlEncode(t, Encoding.UTF8);
                if (t == k)
                {
                    stringBuilder.Append(t);
                }
                else
                {
                    stringBuilder.Append(k.ToUpper());
                }
            }
            return stringBuilder.ToString().Replace("+", "%20").Replace("*", "%2A").Replace("%7E", "~");
        }

        /// <summary>
        /// HMACSHA1签名
        /// </summary>
        /// <param name="accessSecret"></param>
        /// <param name="stringToSign"></param>
        /// <returns></returns>
        private string MySign(string accessSecret, string stringToSign)
        {
            var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(accessSecret));
            var dataBuffer = Encoding.UTF8.GetBytes(stringToSign);
            var hashBytes = hmacsha1.ComputeHash(dataBuffer);
            string stringbyte = BitConverter.ToString(hashBytes, 0).Replace("-", string.Empty).ToLower();
            byte[] bytes = strToToHexByte(stringbyte);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// 消息处理机制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string MessageHandle(string str)
        {
            MessageModel message = JsonHelper.Deserialize<MessageModel>(str);
            string result = "";
            switch (message.Code)
            {
                case "OK":
                    result = "OK";
                    break;

                case "isp.RAM_PERMISSION_DENY":
                    result = "RAM权限DENY";
                    break;

                case "isv.OUT_OF_SERVICE":
                    result = "业务停机";
                    break;

                case "isv.PRODUCT_UN_SUBSCRIPT":
                    result = "未开通云通信产品的阿里云客户";
                    break;

                case "isv.PRODUCT_UNSUBSCRIBE":
                    result = "产品未开通";
                    break;

                case "isv.ACCOUNT_NOT_EXISTS":
                    result = "账户不存在";
                    break;

                case "isv.ACCOUNT_ABNORMAL":
                    result = "账户异常    ";
                    break;

                case "isv.SMS_TEMPLATE_ILLEGAL":
                    result = "短信模板不合法";
                    break;

                case "isv.SMS_SIGNATURE_ILLEGAL":
                    result = "短信签名不合法";
                    break;

                case "isv.INVALID_PARAMETERS":
                    result = "参数异常";
                    break;

                case "isv.MOBILE_NUMBER_ILLEGAL":
                    result = "非法手机号";
                    break;

                case "isv.MOBILE_COUNT_OVER_LIMIT":
                    result = "手机号码数量超过限制";
                    break;

                case "isv.TEMPLATE_MISSING_PARAMETERS":
                    result = "模板缺少变量";
                    break;

                case "isv.BUSINESS_LIMIT_CONTROL":
                    result = "业务限流";
                    break;

                case "isv.INVALID_JSON_PARAM":
                    result = "JSON参数不合法，只接受字符串值";
                    break;

                case "isv.PARAM_LENGTH_LIMIT":
                    result = "参数超出长度限制";
                    break;

                case "isv.PARAM_NOT_SUPPORT_URL":
                    result = "不支持URL";
                    break;

                case "isv.AMOUNT_NOT_ENOUGH":
                    result = "账户余额不足";
                    break;

                case "isv.TEMPLATE_PARAMS_ILLEGAL":
                    result = "模板变量里包含非法关键字";
                    break;
            }
            return result;
        }

        #endregion 调用发送 API
    }

    internal class MessageModel
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}