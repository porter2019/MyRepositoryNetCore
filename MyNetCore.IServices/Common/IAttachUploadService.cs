using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 文件上传操作
    /// </summary>
    public interface IAttachUploadService : IBatchDIServicesTag
    {
        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="files">上传文件列表</param>
        /// <param name="folder">保存的文件夹</param>
        /// <returns></returns>
        List<UploadFileInfo> SaveAttach(IFormFileCollection files, string folder);
    }
}
