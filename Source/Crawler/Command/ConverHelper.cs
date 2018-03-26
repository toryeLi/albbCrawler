using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Command
{
   public class ConvertHelper
    {
        public static string ByteToHex(byte[] data)
        {
            string str = "";

            try
            {

                if ((data != null) && (data.Length > 0))
                {
                    int len = data.Length + 20;
                    StringBuilder strB = new StringBuilder(len);
                    for (int i = 0; i < data.Length; i++)
                    {
                        strB.Append(data[i].ToString("X2"));
                    }
                    str = strB.ToString();
                }
            }
            catch (Exception e)
            {

            }
            return str;
        }
        public static string ByteToString(byte[] data)
        {
            if ((data != null) && (data.Length > 0))
            {
                return Encoding.GetEncoding("GB2312").GetString(data);
            }
            return "";
        }
        public static byte[] HexToByte(string data)
        {
            data = Regex.Replace(data, @"\s", "");
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            int num = data.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = Convert.ToByte(data.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }
        public static byte[] StringToByte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                return Encoding.GetEncoding("GB2312").GetBytes(data);
            }
            return null;
        }
    }
}
