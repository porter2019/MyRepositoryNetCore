using System.Globalization;
using System.Text.RegularExpressions;

namespace MyNetCore.Common.Helper
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public class ValidateHelper
    {
        /// <summary>
        /// 是否为账户名,只能数字、字母、下划线组成
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsAccountName(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z0-9_]*$");
        }

        /// <summary>
        /// 隐藏手机号中间四位为*
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MobileHideCenter(string s)
        {
            return Regex.Replace(s, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
        }

        /// <summary>
        /// 是否为IP
        /// </summary>
        public static bool IsIP(string s)
        {
            return Regex.IsMatch(s, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }

        /// <summary>
        /// 是否是身份证号
        /// </summary>
        public static bool IsIdCard(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (id.Length == 18)
                return CheckIDCard18(id);
            else if (id.Length == 15)
                return CheckIDCard15(id);
            else
                return false;
        }

        /// <summary>
        /// 是否为18位身份证号
        /// </summary>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                return false;//校验码验证

            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 是否为15位身份证号
        /// </summary>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            return true;//符合15位身份证标准
        }

        /// <summary>
		/// 判断字符串是否日期格式：2010-12-31
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsDate(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            return Regex.IsMatch(s, @"^((((19|20)\d{2})-(0?(1|[3-9])|1[012])-(0?[1-9]|[12]\d|30))|(((19|20)\d{2})-(0?[13578]|1[02])-31)|(((19|20)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|((((19|20)([13579][26]|[2468][048]|0[48]))|(2000))-0?2-29))$");
        }

        /// <summary>
        /// 判断字符串是否日期格式：2010-12-31 12:12:12
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateTime(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            return Regex.IsMatch(s, @"^((((19|20)\d{2})-(0?(1|[3-9])|1[012])-(0?[1-9]|[12]\d|30))|(((19|20)\d{2})-(0?[13578]|1[02])-31)|(((19|20)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|((((19|20)([13579][26]|[2468][048]|0[48]))|(2000))-0?2-29))\s+(20|21|22|23|[0-1]\d):[0-5]\d:[0-5]\d$");
        }

        /// <summary>
        /// 判断字符串是否是日期-年格式 2019
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateYear(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            return Regex.IsMatch(s, @"(?:19|20\d{2})");
        }

        /// <summary>
        /// 判断字符串是否是日期-年格式 02
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateMonth(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            return Regex.IsMatch(s, @"^(0?[[1-9]|1[0-2])$");
        }

        /// <summary>
		/// 判断是不是有效的手机号
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsCellPhone(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            Regex regex = new Regex("^1[3456789]\\d{9}$", RegexOptions.IgnoreCase);
            return regex.IsMatch(s);
        }

        /// <summary>
		/// 判断是不是有效的web url
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsWebUrl(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            Regex regex = new Regex(@"^https?:\/\/(([a-zA-Z0-9_-])+(\.)?)*(:\d+)?(\/((\.)?(\?)?=?&?[a-zA-Z0-9_-](\?)?)*)*$", RegexOptions.IgnoreCase);
            return regex.IsMatch(s);
        }

        /// <summary>
        /// 判断是不是有效的金额，不允许为负
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsMoney(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            Regex regex = new Regex(@"^0|(([1-9][0-9]*)|(([0]\.\d{1,2}|[1-9][0-9]*\.\d{1,4})))$", RegexOptions.IgnoreCase);
            return regex.IsMatch(s);
        }

        /// <summary>
        /// 判断是不是有效的金额，允许为负
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsMoneyIncloudMinus(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            Regex regex = new Regex(@"^-?(([1-9][0-9]*)|(([0]\.\d{1,2}|[1-9][0-9]*\.\d{1,4})))$", RegexOptions.IgnoreCase);
            return regex.IsMatch(s);
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            double num;
            return double.TryParse(Convert.ToString(s), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out num);
        }

        /// <summary>
        /// 判断是否为邮箱
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            return regex.IsMatch(s);
        }

        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsZipCode(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            return Regex.IsMatch(s, @"^\d{6}$");
        }

        /// <summary>
        /// 是否是图片文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImgFileName(string fileName)
        {
            if (fileName.IndexOf(".") == -1)
                return false;

            string tempFileName = fileName.Trim().ToLower();
            string extension = tempFileName.Substring(tempFileName.LastIndexOf("."));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }

        /// <summary>
        /// 判断一个ip是否在另一个ip内
        /// </summary>
        /// <param name="sourceIP">检测ip</param>
        /// <param name="targetIP">匹配ip</param>
        /// <returns></returns>
        public static bool InIP(string sourceIP, string targetIP)
        {
            if (string.IsNullOrEmpty(sourceIP) || string.IsNullOrEmpty(targetIP))
                return false;

            string[] sourceIPBlockList = sourceIP.SplitString(".");
            string[] targetIPBlockList = targetIP.SplitString(".");

            int sourceIPBlockListLength = sourceIPBlockList.Length;

            for (int i = 0; i < sourceIPBlockListLength; i++)
            {
                if (targetIPBlockList[i] == "*")
                    return true;

                if (sourceIPBlockList[i] != targetIPBlockList[i])
                {
                    return false;
                }
                else
                {
                    if (i == 3)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个ip是否在另一个ip内
        /// </summary>
        /// <param name="sourceIP">检测ip</param>
        /// <param name="targetIPList">匹配ip列表</param>
        /// <returns></returns>
        public static bool InIPList(string sourceIP, string[] targetIPList)
        {
            if (targetIPList != null && targetIPList.Length > 0)
            {
                foreach (string targetIP in targetIPList)
                {
                    if (InIP(sourceIP, targetIP))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个ip是否在另一个ip内
        /// </summary>
        /// <param name="sourceIP">检测ip</param>
        /// <param name="targetIPStr">匹配ip</param>
        /// <returns></returns>
        public static bool InIPList(string sourceIP, string targetIPStr)
        {
            string[] targetIPList = targetIPStr.SplitString("\n");
            return InIPList(sourceIP, targetIPList);
        }

        /// <summary>
        /// 判断当前时间是否在指定的时间段内
        /// </summary>
        /// <param name="periodList">指定时间段</param>
        /// <param name="liePeriod">所处时间段</param>
        /// <returns></returns>
        public static bool BetweenPeriod(string[] periodList, out string liePeriod)
        {
            if (periodList != null && periodList.Length > 0)
            {
                DateTime startTime;
                DateTime endTime;
                DateTime nowTime = DateTime.Now;
                DateTime nowDate = nowTime.Date;

                foreach (string period in periodList)
                {
                    int index = period.IndexOf("-");
                    startTime = period.Substring(0, index).ObjToDate();
                    endTime = period.Substring(index + 1).ObjToDate();

                    if (startTime < endTime)
                    {
                        if (nowTime > startTime && nowTime < endTime)
                        {
                            liePeriod = period;
                            return true;
                        }
                    }
                    else
                    {
                        if ((nowTime > startTime && nowTime < nowDate.AddDays(1)) || (nowTime < endTime))
                        {
                            liePeriod = period;
                            return true;
                        }
                    }
                }
            }
            liePeriod = string.Empty;
            return false;
        }

        /// <summary>
        /// 判断当前时间是否在指定的时间段内
        /// </summary>
        /// <param name="periodStr">指定时间段</param>
        /// <param name="liePeriod">所处时间段</param>
        /// <returns></returns>
        public static bool BetweenPeriod(string periodStr, out string liePeriod)
        {
            string[] periodList = periodStr.SplitString("\n");
            return BetweenPeriod(periodList, out liePeriod);
        }

        /// <summary>
        /// 判断当前时间是否在指定的时间段内
        /// </summary>
        /// <param name="periodList">指定时间段</param>
        /// <returns></returns>
        public static bool BetweenPeriod(string periodList)
        {
            string liePeriod = string.Empty;
            return BetweenPeriod(periodList, out liePeriod);
        }

        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumericArray(string[] numericStrList)
        {
            if (numericStrList != null && numericStrList.Length > 0)
            {
                foreach (string numberStr in numericStrList)
                {
                    if (!IsNumeric(numberStr))
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumericRule(string numericRuleStr, string splitChar)
        {
            return IsNumericArray(numericRuleStr.SplitString(splitChar));
        }

        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumericRule(string numericRuleStr)
        {
            return IsNumericRule(numericRuleStr, ",");
        }
    }
}