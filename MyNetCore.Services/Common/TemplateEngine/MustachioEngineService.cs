using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.Common;

namespace MyNetCore.Services.Common.TemplateEngine
{
    /// <summary>
    /// 使用Mustachio引擎生成代码
    /// </summary>
    [ServiceLifetime(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class MustachioEngineService : ITemplateEngineService
    {
        /// <summary>
        /// 根据模板解析成最终的字符串
        /// </summary>
        /// <param name="templateFile">模板绝对路径</param>
        /// <param name="model">Dictionary<string,object>类型</param>
        /// <returns></returns>
        public async Task<string> ParseAsync(string templateFile, object model)
        {
            if (!System.IO.File.Exists(templateFile)) throw new Exception($"模板文件不存在:{templateFile}");
            var templateContent = await System.IO.File.ReadAllTextAsync(templateFile);
            var template = Mustachio.Parser.Parse(templateContent, new Mustachio.ParsingOptions() { DisableContentSafety = true });

            Dictionary<string, object> dic = (Dictionary<string, object>)model;
            var content = (string)template(dic);
            //{{#HasView}}
            //有视图：{{../ViewClassName}}
            //{{/HasView}}
            //{{^HasView}}
            //没有视图：{{../ModelName}}
            //{{/HasView}}
            return content;
        }
    }
}
