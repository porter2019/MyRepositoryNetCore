using System;

namespace MyNetCore
{
    public class CustomException : ApplicationException
    {
        private string error;
        private Exception innerException;
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public CustomException()
        {

        }
        /// <summary>
        /// 带一个字符串参数的构造函数，作用：当程序员用Exception类获取异常信息而非 MyException时把自定义异常信息传递过去
        /// </summary>
        /// <param name="msg"></param>
        public CustomException(string msg) : base(msg)
        {
            this.error = msg;
        }
        /// <summary>
        /// 带有一个字符串参数和一个内部异常信息参数的构造函数
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public CustomException(string msg, Exception innerException) : base(msg, innerException)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }
}
