using Microsoft.AspNetCore.Http;
using System.IO;

namespace MyNetCore.Services
{
    /// <summary>
    /// 附件上传服务
    /// </summary>
    [ServiceLifetime()]
    public class AttachUploadService : IAttachUploadService
    {
        private readonly IConfiguration _config;

        public AttachUploadService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="files">上传文件列表</param>
        /// <param name="folder">保存的文件夹</param>
        /// <returns></returns>
        public List<UploadFileInfo> SaveAttach(IFormFileCollection files, string folder)
        {
            if (files == null) return default;

            if (string.IsNullOrWhiteSpace(folder)) folder = "attach";
            var baseIODirectory = _config[GlobalVar.ConfigKeyPath_StaticFilesDirectoryKey];
            var domain = _config[GlobalVar.ConfigKeyPath_StaticFileDomainUrl];
            var baseRootFolder = "uploads";
            var saveFolder = baseRootFolder + "/" + folder;   // 保存的相对目录 /uploads/attach

            List<UploadFileInfo> fileList = new();
            foreach (var file in files)
            {
                var fileExt = Path.GetExtension(file.FileName).ToLower(); //文件后缀名  .jpg
                if (string.IsNullOrWhiteSpace(fileExt) && file.FileName == "blob") fileExt = ".jpg";
                var tempFileIOFolder = Path.Combine(baseIODirectory, baseRootFolder, folder);
                if (!Directory.Exists(tempFileIOFolder)) Directory.CreateDirectory(tempFileIOFolder);
                var tempFileIOPath = tempFileIOFolder + Guid.NewGuid().ToString("N") + fileExt;
                //保存
                using (FileStream fs = new(tempFileIOPath, FileMode.Create))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }

                var fileMD5 = FileHelper.GetMD5HashFromFile(tempFileIOPath);
                var md5CR32 = CRC32.GetCRC32Str(fileMD5);//使用CR32是为了缩短用md5做文件名太长

                var finalFileIOPath = Path.Combine(baseIODirectory, baseRootFolder, folder, md5CR32 + fileExt);
                if (File.Exists(finalFileIOPath))
                {
                    File.Delete(tempFileIOPath);
                }
                else
                {
                    File.Move(tempFileIOPath, finalFileIOPath, true);
                }

                var finalFilePath = "/" + saveFolder + "/" + md5CR32 + fileExt;

                fileList.Add(new UploadFileInfo()
                {
                    FileExt = fileExt,
                    FilePath = finalFilePath,
                    FileSourceName = file.FileName,
                    FileSize = file.Length,
                    FileType = file.ContentType,
                    FileName = file.FileName,//md5CR32 + fileExt,
                    FileWebPath = domain + finalFilePath
                });
            }

            return fileList;
        }
    }
}