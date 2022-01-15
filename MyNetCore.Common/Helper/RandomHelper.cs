namespace MyNetCore.Common.Helper
{
    /// <summary>
    /// 随机数工具类
    /// </summary>
    public static class RandomHelper
    {
        //随机数
        private static char[] constant ={
                                            '0','1','2','3','4','5','6','7','8','9',
                                            'a','b','c','d','e','f','g','h','i','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z',
                                            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z'
                                          };

        /// <summary>
        /// 整形随机数
        /// </summary>
        private static int[] constantInt = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        /// 生成随机数,数字+小写字母+大写字母随机组合
        /// </summary>
        /// <param name="Length">长度</param>
        /// <returns></returns>
        public static string GenerateNumber(int Length)
        {
            StringBuilder newRandom = new StringBuilder(62);
            Random rd = new();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 生成整形随机数
        /// </summary>
        /// <param name="Length">长度</param>
        /// <returns></returns>
        public static string GenerateIntNumber(int Length)
        {
            StringBuilder newRandom = new StringBuilder(62);
            Random rd = new();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constantInt[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
    }
}