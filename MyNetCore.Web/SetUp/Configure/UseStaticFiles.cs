using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;

namespace MyNetCore.Web.SetUp
{
    public static class UseStaticFiles
    {
        /// <summary>
        /// 添加静态文件支持
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void UseMyStaticFiles([NotNull] this IApplicationBuilder app, IConfiguration config)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            //拓展MIME支持
            var unknownFileOption = new FileExtensionContentTypeProvider();
            unknownFileOption.Mappings[".myapp"] = "application/x-msdownload";
            unknownFileOption.Mappings[".htm3"] = "text/html";
            unknownFileOption.Mappings[".image"] = "image/png";
            unknownFileOption.Mappings[".rtf"] = "application/x-msdownload";
            unknownFileOption.Mappings[".apk"] = "application/vnd.android.package-archive";
            unknownFileOption.Mappings[".rar"] = "application/octet-stream";
            unknownFileOption.Mappings[".7z"] = "application/octet-stream";

            var staticDirectory = config[GlobalVar.ConfigKeyPath_StaticFilesDirectoryKey];
            if (staticDirectory.IsNull()) return;

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = unknownFileOption,
                FileProvider = new PhysicalFileProvider(staticDirectory)
            });
        }
    }
}