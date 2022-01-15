namespace MyNetCore.Model.ResponseModel
{
    /// <summary>
    /// 登录后返回的用户信息
    /// </summary>
    public class LoginUserInfo
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间，Unix时间戳，精度到秒，前端直接let a = Date.parse(new Date()) / 1000; 然后判断a 《  FailureTime就行了
        /// </summary>
        public int FailureTime { get; set; }
    }
}