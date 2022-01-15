using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

/**
 * https://github.com/codebude/QRCoder
 */

namespace MyNetCore.Services
{
    /// <summary>
    /// 二维码服务实现类
    /// </summary>
    [ServiceLifetime(false)]
    public class QRCodeService : IQRCodeService
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public Task<byte[]> GenerateQRCode(string content)
        {
            var generator = new QRCodeGenerator();

            var codeData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true);
            QRCode qrcode = new QRCode(codeData);

            var bitmapImg = qrcode.GetGraphic(10, Color.Black, Color.White, true);
            using MemoryStream stream = new MemoryStream();
            bitmapImg.Save(stream, ImageFormat.Jpeg);
            return Task.FromResult(stream.GetBuffer());
        }

        /// <summary>
        /// 生成二维码，指定中间的logo
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="iconPath">图片物理路径地址，获取wwwroot下_hostEnvironment.ContentRootPath</param>
        /// <returns></returns>
        public Task<byte[]> GenerateQRCode(string content, string iconPath)
        {
            var generator = new QRCodeGenerator();

            var codeData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true);
            QRCode qrcode = new QRCode(codeData);

            var bitmapImg = qrcode.GetGraphic(10, Color.Black, Color.White, new Bitmap(iconPath));
            // int pixelsPerModule 生成二维码图片的像素大小
            // Color darkColor 暗色 一般设置为Color.Black 黑色
            // Color lightColor 亮色 一般设置为Color.White 白色
            // Bitmap icon 二维码水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png"));默认为NULL ，加上这个二维码中间会显示一个图标
            // int iconSizePercent 水印图标的大小比例 ，可根据自己的喜好设置
            // int iconBorderWidth 水印图标的边框
            // bool drawQuietZones 静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息，即是否绘画二维码的空白边框区域 默认为true
            using MemoryStream stream = new MemoryStream();
            bitmapImg.Save(stream, ImageFormat.Jpeg);
            return Task.FromResult(stream.GetBuffer());
        }
    }
}