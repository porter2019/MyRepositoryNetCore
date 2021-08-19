namespace MyNetCore.IServices
{
    /// <summary>
    /// JWT操作服务
    /// </summary>
    public interface IJwtService : IBatchDIServicesTag
    {
        /// <summary>
        /// 生成Jwt Token
        /// </summary>
        /// <typeparam name="T">payload载体对象类型</typeparam>
        /// <param name="payload">数据</param>
        /// <returns></returns>
        string GenerateToken<T>(T payload);

        /// <summary>
        /// 根据jwtToken获取payload对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="token">jwt token字符串</param>
        /// <returns></returns>
        T ParseToken<T>(string token);
    }
}
