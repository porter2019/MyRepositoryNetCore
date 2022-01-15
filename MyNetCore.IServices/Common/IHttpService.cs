namespace MyNetCore.IServices
{
    /// <summary>
    /// Http服务
    /// </summary>
    public interface IHttpService : IBatchDIServicesTag
    {
        /// <summary>
        /// 发送HttpPost请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="jsonParam">Json格式的post参数</param>
        /// <returns></returns>
        Task<string> PostAsync(string url, string jsonParam);

        /// <summary>
        /// 发送HttpPost请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="jsonParam">Json格式的post参数</param>
        /// <returns></returns>
        string Post(string url, string jsonParam);

        /// <summary>
        /// 发送HttpGet请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        Task<string> GetAsync(string url);

        /// <summary>
        /// 发送HttpGet请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        string Get(string url);

        /// <summary>
        /// 发送HttpDelete请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        Task<string> DeleteAsync(string url);

        /// <summary>
        /// 发送HttpDelete请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        string Delete(string url);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="baseDirectory">保存文件的根目录D:\test</param>
        /// <param name="fileWebUrl">文件web地址</param>
        /// <param name="relativePath">保存的相对路径/uploads/aa</param>
        /// <param name="fileName">保存的文件名aa.zip</param>
        /// <returns>本地是否有下载的文件，如果有，则不下载</returns>
        bool DownloadFileAsync(string baseDirectory, string fileWebUrl, string relativePath, string fileName);
    }
}
