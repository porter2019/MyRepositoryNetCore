using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore
{
    /// <summary>
    /// 接口返回对象
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ApiCode code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据条数 分页用
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 枚举集合
        /// </summary>
        public List<ApiEnumProp> enums { get; set; }

        /// <summary>
        /// 判断当前返回值是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsOK() => code == ApiCode.成功;

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult OK()
        {
            return new ApiResult()
            {
                code = ApiCode.成功,
                msg = "OK"
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回的对象</param>
        /// <returns></returns>
        public static ApiResult OK(object data)
        {

            return new ApiResult()
            {
                code = ApiCode.成功,
                data = data,
                msg = "OK"
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回的对象</param>
        /// <param name="enums">枚举</param>
        /// <returns></returns>
        public static ApiResult OK(object data, List<ApiEnumProp> enums)
        {

            return new ApiResult()
            {
                code = ApiCode.成功,
                data = data,
                enums = enums,
                msg = "OK"
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <returns></returns>
        public static ApiResult OK(string msg)
        {
            return new ApiResult()
            {
                code = ApiCode.成功,
                msg = msg
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的对象</param>
        /// <returns></returns>
        public static ApiResult OK(string msg, object data)
        {
            return new ApiResult()
            {
                code = ApiCode.成功,
                data = data,
                msg = msg
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult OK(long total, object data)
        {
            return new ApiResult()
            {
                code = ApiCode.成功,
                total = total,
                data = data,
                msg = "OK"
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="total">数据总条数</param>
        /// <param name="data">返回数据</param>
        /// <param name="enums">枚举</param>
        /// <returns></returns>
        public static ApiResult OK(long total, object data, List<ApiEnumProp> enums)
        {
            return new ApiResult()
            {
                code = ApiCode.成功,
                total = total,
                data = data,
                enums = enums,
                msg = "OK"
            };
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult ValidateFail()
        {
            return new ApiResult()
            {
                code = ApiCode.验证失败,
                msg = "验证失败"
            };
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResult ValidateFail(string msg)
        {
            return new ApiResult()
            {
                code = ApiCode.验证失败,
                msg = msg
            };
        }

        /// <summary>
        /// 验证失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResult ValidateFail(string msg, object data)
        {
            return new ApiResult()
            {
                code = ApiCode.验证失败,
                msg = msg,
                data = data
            };
        }

        /// <summary>
        /// 请求异常，400客户端问题
        /// </summary>
        /// <returns></returns>
        public static ApiResult Failed()
        {
            return new ApiResult()
            {
                code = ApiCode.失败,
                msg = "请求异常"
            };
        }

        /// <summary>
        /// 请求异常，400客户端问题
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public static ApiResult Failed(string msg)
        {
            return new ApiResult()
            {
                code = ApiCode.失败,
                msg = msg
            };
        }

        /// <summary>
        /// 服务器异常
        /// </summary>
        /// <returns></returns>
        public static ApiResult Error()
        {
            return new ApiResult()
            {
                code = ApiCode.系统异常,
                msg = "系统异常，请联系系统管理员"
            };
        }

        /// <summary>
        /// 服务器异常
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public static ApiResult Error(string msg)
        {
            return new ApiResult()
            {
                code = ApiCode.系统异常,
                msg = msg
            };
        }

        /// <summary>
        /// 404
        /// </summary>
        /// <returns></returns>
        public static ApiResult NotFound()
        {
            return new ApiResult()
            {
                code = ApiCode.不存在,
                msg = "数据不存在或者已删除"
            };
        }

        /// <summary>
        /// 404
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResult NotFound(string msg)
        {
            return new ApiResult()
            {
                code = ApiCode.不存在,
                msg = msg
            };
        }

        /// <summary>
        /// 接口未实现
        /// </summary>
        /// <returns></returns>
        public static ApiResult Unrealized()
        {
            return new ApiResult()
            {
                code = ApiCode.未实现,
                msg = "接口未实现"
            };
        }

        /// <summary>
        /// 没有权限
        /// </summary>
        /// <returns></returns>
        public static ApiResult Forbidden()
        {
            return new ApiResult()
            {
                code = ApiCode.没有权限,
                msg = "没有权限"
            };
        }

        /// <summary>
        /// 无效凭证
        /// </summary>
        /// <returns></returns>
        public static ApiResult PreconditionFailed()
        {
            return new ApiResult()
            {
                code = ApiCode.无效凭证,
                msg = "无效凭证"
            };
        }

        /// <summary>
        /// 用户未登录
        /// </summary>
        /// <returns></returns>
        public static ApiResult Anonymous()
        {
            return new ApiResult()
            {
                code = ApiCode.未登录,
                msg = "用户未登录"
            };
        }


    }

    /// <summary>
    /// API-枚举集合
    /// </summary>
    public class ApiEnumProp
    {
        public ApiEnumProp()
        {

        }

        public ApiEnumProp(string name, List<ApiEnumOptions> options)
        {
            this.Name = name;
            this.Options = options;
        }

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; }

        public List<ApiEnumOptions> Options { get; set; }
    }

    /// <summary>
    /// 枚举-项
    /// </summary>
    public class ApiEnumOptions
    {
        public ApiEnumOptions(string label, int value)
        {
            this.Label = label;
            this.Value = value;
        }

        public string Label { get; set; }

        public int Value { get; set; }
    }

    /// <summary>
    /// HTTP返回状态码
    /// </summary>
    public enum ApiCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        成功 = 200,
        /// <summary>
        /// 验证失败
        /// </summary>
        验证失败 = 203,
        /// <summary>
        /// 失败
        /// </summary>
        失败 = 400,
        /// <summary>
        /// 系统异常
        /// </summary>
        系统异常 = 500,
        /// <summary>
        /// 未登录
        /// </summary>
        未登录 = 401,
        /// <summary>
        /// 没有权限
        /// </summary>
        没有权限 = 403,
        /// <summary>
        /// 无效凭证
        /// </summary>
        无效凭证 = 412,
        /// <summary>
        /// 不存在
        /// </summary>
        不存在 = 404,
        /// <summary>
        /// 未实现
        /// </summary>
        未实现 = 410,

    };

}
