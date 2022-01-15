namespace MyNetCore
{
    /// <summary>
    /// JWT token信息
    /// </summary>
    public class JwtTokenObject
    {
        /// <summary>
		/// 用户标识
		/// </summary>
		public int open_id { get; set; }

        /// <summary>
        /// 刷新凭证
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间，时间戳(秒)
        /// </summary>
        public long exp { get; set; }

        /// <summary>
        /// json数据
        /// </summary>
        public string json_data { get; set; }
    }
}