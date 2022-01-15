using RestSharp;
using System.IO;

namespace MyNetCore.Services
{
    /// <summary>
    /// Http服务实现
    /// </summary>
    [ServiceLifetime()]
    public class HttpService : IHttpService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private static readonly string _headFrom = "RestSharp";

        public HttpService(ILogger<HttpService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// 发送HttpPost请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="jsonParam">Json格式的post参数</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, string jsonParam)
        {
            try
            {
                var uri = new Uri(url);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var request = new RestRequest(uri.PathAndQuery, Method.POST);
                request.AddJsonBody(jsonParam);
                request.AddHeader("from", _headFrom);
                var response = await restClient.ExecuteAsync(request);
                var responseContent = response.Content;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HttpPost方式请求数据返回值错误，状态码：{(int)response.StatusCode}，返回值：{responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HttpPost方式请求数据出现错误");
                return JsonHelper.Serialize(ApiResult.Error("服务接口调用失败"));
            }
        }

        /// <summary>
        /// 发送HttpPost请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="jsonParam">Json格式的post参数</param>
        /// <returns></returns>
        public string Post(string url, string jsonParam)
        {
            try
            {
                var uri = new Uri(url);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var request = new RestRequest(uri.PathAndQuery, Method.POST);
                request.AddJsonBody(jsonParam);
                request.AddHeader("from", _headFrom);
                var response = restClient.Execute(request);
                var responseContent = response.Content;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HttpPost方式请求数据返回值错误，状态码：{(int)response.StatusCode}，返回值：{responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HttpPost方式请求数据出现错误");
                return JsonHelper.Serialize(ApiResult.Error("服务接口调用失败"));
            }
        }

        /// <summary>
        /// 发送HttpGet请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url)
        {
            try
            {
                var uri = new Uri(url);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var request = new RestRequest(uri.PathAndQuery, Method.GET);
                request.AddHeader("from", _headFrom);
                var response = await restClient.ExecuteAsync(request);
                var responseContent = response.Content;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HttpGet方式请求数据返回值错误，状态码：{(int)response.StatusCode}，返回值：{responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HttpGet方式请求数据出现错误");
                return JsonHelper.Serialize(ApiResult.Error("服务接口调用失败"));
            }
        }

        /// <summary>
        /// 发送HttpGet请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string Get(string url)
        {
            try
            {
                var uri = new Uri(url);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var request = new RestRequest(uri.PathAndQuery, Method.GET);
                request.AddHeader("from", _headFrom);
                var response = restClient.Execute(request);
                var responseContent = response.Content;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HttpGet方式请求数据返回值错误，状态码：{(int)response.StatusCode}，返回值：{responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HttpGet方式请求数据出现错误");
                return JsonHelper.Serialize(ApiResult.Error("服务接口调用失败"));
            }
        }

        /// <summary>
        /// 发送HttpDelete请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string url)
        {
            try
            {
                var uri = new Uri(url);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var request = new RestRequest(uri.PathAndQuery, Method.DELETE);
                request.AddHeader("from", _headFrom);
                var response = await restClient.ExecuteAsync(request);
                var responseContent = response.Content;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HttpDelete方式请求数据返回值错误，状态码：{(int)response.StatusCode}，返回值：{responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HttpDelete方式请求数据出现错误");
                return JsonHelper.Serialize(ApiResult.Error("服务接口调用失败"));
            }
        }

        /// <summary>
        /// 发送HttpDelete请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string Delete(string url)
        {
            try
            {
                var uri = new Uri(url);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var request = new RestRequest(uri.PathAndQuery, Method.DELETE);
                request.AddHeader("from", _headFrom);
                var response = restClient.Execute(request);
                var responseContent = response.Content;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HttpDelete方式请求数据返回值错误，状态码：{(int)response.StatusCode}，返回值：{responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HttpDelete方式请求数据出现错误");
                return JsonHelper.Serialize(ApiResult.Error("服务接口调用失败"));
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="baseDirectory">保存文件的根目录D:\test</param>
        /// <param name="fileWebUrl">文件web地址</param>
        /// <param name="relativePath">保存的相对路径/uploads/aa</param>
        /// <param name="fileName">保存的文件名aa.zip</param>
        /// <returns>本地是否有下载的文件，如果有，则不下载</returns>
        public bool DownloadFileAsync(string baseDirectory, string fileWebUrl, string relativePath, string fileName)
        {
            var fullIOdirectory = baseDirectory + relativePath;
            if (!Directory.Exists(fullIOdirectory)) Directory.CreateDirectory(fullIOdirectory);
            if (File.Exists(fullIOdirectory + fileName)) return true;

            using (var writer = File.OpenWrite(fullIOdirectory + fileName))
            {
                var req = new RestRequest(fileWebUrl, Method.GET)
                {
                    ResponseWriter = responseStream =>
                    {
                        using (responseStream)
                        {
                            responseStream.CopyTo(writer);
                        }
                    }
                };
                var uri = new Uri(fileWebUrl);
                var origin = uri.GetLeftPart(UriPartial.Authority);
                var restClient = new RestClient(origin);
                var response = restClient.DownloadData(req);
            }

            return File.Exists(fullIOdirectory + fileName);
        }
    }
}
