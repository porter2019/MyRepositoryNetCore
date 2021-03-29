namespace MyNetCore
{
    /// <summary>
    /// 上传附件返回对象
    /// </summary>
    public class UploadFileInfo
    {
        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 包含域名的访问地址
        /// </summary>
        public string FileWebPath { get; set; }

        /// <summary>
        /// 文件名(上传时的文件名)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件原始名称
        /// </summary>
        public string FileSourceName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件拓展名
        /// </summary>
        public string FileExt { get; set; }

    }
}
