namespace MyNetCore.IServices
{
    /// <summary>
    /// 模板引擎
    /// </summary>
    public interface ITemplateEngineService : IBatchDIServicesTag
    {
        /// <summary>
        /// 根据模板解析成最终的字符串
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> ParseAsync(string templateFile, object model);
    }
}