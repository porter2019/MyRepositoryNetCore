using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.Common.Helper;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Services
{
    /// <summary>
    /// 阿里云发送短信
    /// </summary>
    [ServiceLifetime()]
    public class AliSMSServices : ISMSServices
    {
        private readonly ICacheServices _cacheServices;
        private readonly IValidateCodeHistoryServices _validateCodeHistoryServices;
        private readonly ILogger _logger;

        public AliSMSServices(ICacheServices cacheServices, IValidateCodeHistoryServices validateCodeHistoryServices, ILogger<AliSMSServices> logger)
        {
            _cacheServices = cacheServices;
            _validateCodeHistoryServices = validateCodeHistoryServices;
            _logger = logger;
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

            var sendResult = AliSMSHelper.Send(mobile, jsonData, "SMS_128965131");

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
        async Task<bool> CheckSendIsOftenAsync(string mobile)
        {
            var sendTime = await _cacheServices.GetAsync(mobile);
            if (sendTime.IsNull()) return false;
            _logger.LogInformation("缓存中的:" + sendTime);
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
        async Task<ApiResult> SaveSendHistoryAsync(ApiResult sendResult, string sendBody, string guid, string mobile, string code)
        {
            if (sendResult.code != ApiCode.成功) return ApiResult.Error(sendResult.msg);

            //用于校验验证码是否正确
            await _cacheServices.AddAsync(guid, code, 60 * 10);
            //用于校验是否发送频繁
            await _cacheServices.AddAsync(mobile, DateTime.Now.ToTimeStamp(), 60);

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

        #endregion
    }
}
