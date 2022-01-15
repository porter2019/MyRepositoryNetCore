namespace MyNetCore.Model.Entity
{
    /// <summary>
    /// 附件表
    /// </summary>
    [FsTable("附件表")]
    public class CommonAttach : BaseEntity
    {
        [FsColumn("主键id", IsPK = true, Position = 1)]
        public int AttachId { get; set; }

        /// <summary>
        /// 关联的数据id
        /// </summary>
        [FsColumn("关联的数据id")]
        public int RefId { get; set; }

        /// <summary>
        /// 关联的数据模型
        /// </summary>
        [FsColumn("关联的数据模型")]
        public string RefModel { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [FsColumn("文件名称", false, 255)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [FsColumn("文件类型", false, 255)]
        public string FileType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [FsColumn("文件大小")]
        public long FileSize { get; set; }

        /// <summary>
        /// 文件后缀名
        /// </summary>
        [FsColumn("文件后缀名", false, 50)]
        public string FileExt { get; set; }

        /// <summary>
        /// 所属字段
        /// </summary>
        [FsColumn("所属字段", true, 200)]
        public string Field { get; set; } = "Attach";

        /// <summary>
        /// 文件相对路径
        /// </summary>
        [FsColumn("文件路径", true, 200)]
        public string FilePath { get; set; }

        /// <summary>
        /// 完整路径
        /// </summary>
        public string FileWebPath
        {
            get
            {
                if (FilePath.IsNull()) return "";
                try
                {
                    var domainName = _config[GlobalVar.ConfigKeyPath_StaticFileDomainUrl];
                    return domainName + FilePath;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("视图中获取配置文件内容失败:" + ex.Message);
                    return "";
                }
            }
        }

        public string name
        {
            get
            {
                return FileName;
            }
        }

        public string url
        {
            get
            {
                return FileWebPath;
            }
        }
    }
}