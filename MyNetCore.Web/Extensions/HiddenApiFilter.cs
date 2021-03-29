using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyNetCore.Web
{
    /// <summary>
    /// 隐藏Swagger接口属性标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenApiAttribute : Attribute { }

    /// <summary>
    /// 自定义Swagger隐藏过滤器
    /// </summary>
    public class HiddenApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {

                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute))
                            || method.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute)))
                    {
                        string key = "/" + apiDescription.RelativePath;
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        swaggerDoc.Paths.Remove(key);
                    }
                }

            }

        }
    }

    /// <summary>
    /// 自定义Swagger隐藏参数过滤器
    /// </summary>
    public class HiddenApiSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var excludeProperties = new[] { "currentUserId", "currentUserName", "where", "orderBy" };

            foreach (var prop in excludeProperties)
                if (schema.Properties.ContainsKey(prop))
                    schema.Properties.Remove(prop);
        }
    }
}
