using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 二维码生成服务
    /// </summary>
    public interface IQRCodeServices : IBatchDIServicesTag
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        Task<byte[]> GenerateQRCode(string content);

        /// <summary>
        /// 生成二维码，指定中间的logo
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="iconPath">图片物理路径地址，获取wwwroot下_hostEnvironment.ContentRootPath</param>
        /// <returns></returns>
        Task<byte[]> GenerateQRCode(string content, string iconPath);

    }
}
