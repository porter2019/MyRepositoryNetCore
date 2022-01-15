namespace MyNetCore
{
    public class CRC32
    {
        private static ulong[] Crc32Table;

        private CRC32()
        { }

        /// <summary>
        /// 生成CRC32码表
        /// </summary>
        private static void GetCRC32Table()
        {
            ulong Crc;
            Crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    else
                        Crc >>= 1;
                }
                Crc32Table[i] = Crc;
            }
        }

        /// <summary>
        /// 获取字符串的CRC32校验值
        /// </summary>
        /// <param name="sInputString"></param>
        /// <returns></returns>
        public static string GetCRC32Str(string sInputString)
        {
            GetCRC32Table();
            byte[] buffer = Encoding.ASCII.GetBytes(sInputString); ulong value = 0xffffffff;
            int len = buffer.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            return (value ^ 0xffffffff).ToString();
        }
    }
}