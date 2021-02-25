namespace MyNetCore.Common.Helper
{
    /// <summary>
    /// 用于在类库中打印日志
    /// </summary>
    public class NlogTraceListener : System.Diagnostics.TraceListener
    {
        /// <summary>
        /// Write输出Info
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(message);
        }

        /// <summary>
        /// WriteLine输出Error
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(message);
        }

        void TxtToFile(string message)
        {
            //if (string.IsNullOrWhiteSpace(message)) return;
            //string file_name = "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //string server_path = "\\logs\\";
            //string wl_path = System.Threading.Thread.GetDomain().BaseDirectory + server_path;
            //if (!Directory.Exists(wl_path))
            //    Directory.CreateDirectory(wl_path); //如果没有该目录，则创建
            //StreamWriter sw = new StreamWriter(wl_path + file_name, true, System.Text.Encoding.UTF8);
            //DateTime dt = DateTime.Now;
            //sw.WriteLine("**************************" + dt.ToString() + " begin **************************");
            //sw.WriteLine(message);
            //sw.WriteLine("/*************************" + dt.ToString() + " end **************************/");
            //sw.Close();
        }
    }
}
