using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Command
{
   public class Tool
    {
        public static bool Contains(string input, string key)
        {
            return input.Contains(key);
        }

        //public static bool Check(string input, string rules)
        //{
        //    string pattern = RegexHelper.GetPattern(rules);
        //    string[] groupValue = RegexHelper.GetGroupValue(input, pattern);

        //    if (groupValue.Length>=1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        #region 校验 暂时不用
        //public static string CreateCCITTChecksum(string data)
        //{
        //    if (string.IsNullOrEmpty(data))
        //    {
        //        return "";
        //    }
        //    return CRCHelper.CRC16(data, 0x8408);
        //}

        //public static string CreateModbusChecksum(string data)
        //{
        //    if (string.IsNullOrEmpty(data))
        //    {
        //        return "";
        //    }
        //    return CRCHelper.CRC16(data, 0xa001);
        //}
        #endregion

        #region  10进制转 2进制串、16进制串、浮点数转16进制
        public static string DecToBin(object input)
        {
            string str = "";
            try
            {
                str = Convert.ToString(int.Parse(input.ToString().Trim()), 2);
            }
            catch
            {
            }

            return str;
        }

        public static string DecToHex(object input)
        {
            string str = "";
            try
            {
                int result = 0;
                if ((input != null) && int.TryParse(input.ToString(), out result))
                {
                    str = Convert.ToString(result, 0x10);
                }
            }
            catch
            {
            }

            return str;
        }

        /// <summary>
        /// 浮点数转16进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DecToHex4B(object input)
        {
            string str = "";
            try
            {
                string str2 = ConvertHelper.ByteToHex(BitConverter.GetBytes(float.Parse(input.ToString())));
                if (str2.Length == 8)
                {
                    string str3 = "";
                    for (int i = 0; i < 4; i++)
                    {
                        str3 = str3 + str2.Substring(8 - ((i + 1) * 2), 2);
                    }
                    str = str3;
                }
            }
            catch
            {
            }

            return str;
        }
        #endregion

        public static void Delay(int delayTime)
        {

            Thread.Sleep(delayTime);
        }

        public static string DeleteEmpty(string input)
        {
            string str = "";
            if (!string.IsNullOrEmpty(input))
            {
                str = Regex.Replace(input, @"\s", "");
            }

            return str;
        }

        public static decimal FloatToDec(object input)
        {
            decimal num = -999M;
            try
            {
                num = decimal.Parse(input.ToString().Trim(), NumberStyles.Float);
            }
            catch
            {
            }

            return num;
        }

        public static string FormatDateTime(object dateTime)
        {
            string str = "";
            try
            {
                str = DateTime.Parse(dateTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            return str;
        }

        public static string FormatDateTime(object dateTime, string outFormat)
        {
            string str = "";
            try
            {
                str = DateTime.Parse(dateTime.ToString()).ToString(outFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            return str;
        }

        //public static string FormatDateTime(object dateTime, string preFormat, string outFormat)
        //{
        //    string str = "";
        //    try
        //    {
        //        str = DateTime.ParseExact(dateTime.ToString(), preFormat, CultureInfo.InvariantCulture).ToString(outFormat);
        //    }
        //    catch
        //    {
        //    }
        //    NotifyManager.ShowMsg(FormatHelper.FormatString(string.Format("格式化时间 <{0}> <{1},{2}>：<{3}> = <{4}>", new object[] { "FormatDateTime", preFormat, outFormat, dateTime, str })));
        //    return str;
        //}

        public static string FormatDec(object data, int intBit, int decBit)
        {
            string str = "";
            try
            {
                int num2;
                decimal num = decimal.Parse(data.ToString());
                StringBuilder builder = new StringBuilder();
                for (num2 = 0; num2 < intBit; num2++)
                {
                    builder.Append("0");
                }
                if (decBit > 0)
                {
                    builder.Append(".");
                    for (num2 = 0; num2 < decBit; num2++)
                    {
                        builder.Append("0");
                    }
                }
                str = string.Format(string.Format("{{0:{0}}}", builder.ToString()), num);
            }
            catch
            {
            }

            return str;
        }





        #region 2进制转换相关
        /// <summary>
        /// 2进制转 int型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int BinToDec(string input)
        {
            int num = -999;
            try
            {
                num = Convert.ToInt32(input.Trim(), 2);
            }
            catch
            {
            }

            return num;
        }
        /// <summary>
        /// 2进制串转16进制串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string BinToHex(string input)
        {
            string str = "";
            try
            {
                str = Convert.ToString(Convert.ToInt32(input.Trim(), 2), 0x10);
            }
            catch
            {
            }

            return str;
        }
        #endregion
        #region  16进制转换为 2进制串、10进制、ASCII字符
        /// <summary>
        /// Hex转2进制
        /// </summary>
        /// <param name="input">Hex串</param>
        /// <returns></returns>
        public static string HexToBin(string input)
        {
            string str = "";
            try
            {
                input = Regex.Replace(input, @"\s", "");
                str = Convert.ToString(Convert.ToInt32(input, 0x10), 2);
                str = str.PadLeft(input.Length * 4, '0');
            }
            catch
            {
            }

            return str;
        }
        public static int HexToDec(string input)
        {
            return HexToDec(input, "int32");
        }

        /// <summary>
        /// Hex转10进制数
        /// </summary>
        /// <param name="input">Hex串</param>
        /// <returns></returns>
        public static int HexToDec(string input, string dType = "int32")
        {
            int num = -999;
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    input = Regex.Replace(input, @"\s", "");
                    if (!string.IsNullOrEmpty(input))
                    {
                        if (dType == "int32")
                        {
                            num = Convert.ToInt32(input, 0x10);
                        }
                        else
                        {
                            num = Convert.ToInt16(input, 0x10);
                        }

                    }
                }
            }
            catch
            {
            }

            return num;
        }
        /// <summary>
        /// Hex转ASCII字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HexToString(string input)
        {
            string str = "";
            try
            {
                str = ConvertHelper.ByteToString(ConvertHelper.HexToByte(input));
            }
            catch
            {
            }

            return str;
        }

        /// <summary>
        /// 4字节码Hex转浮点数
        /// </summary>
        /// <param name="input">Hex串</param>
        /// <returns></returns>
        public static decimal Hex4BToDec(string input)
        {
            decimal num = -999M;
            byte[] buffer = new byte[4];
            try
            {
                input = Regex.Replace(input, @"\s", "");
                for (int i = 0; i < 4; i++)
                {
                    buffer[i] = Convert.ToByte(input.Substring((3 - i) * 2, 2), 0x10);
                }
                num = Convert.ToDecimal(BitConverter.ToSingle(buffer, 0));
            }
            catch
            {
            }

            return num;
        }

        #endregion


        //public static string ReverseHex(string input)
        //{
        //    string str = "";
        //    try
        //    {
        //        str = ConvertHelper.ByteToHex(ConvertHelper.HexToByte(input).Reverse<byte>().ToArray<byte>());
        //    }
        //    catch
        //    {
        //    }

        //    return str;
        //}

        //public static string[] Split(string input, string rules)
        //{
        //    string pattern = RegexHelper.GetPattern(rules);
        //    string[] groupValue = RegexHelper.GetGroupValue(input, pattern);

        //    return groupValue;
        //}

        public static string StringToHex(string input)
        {
            string str = "";
            try
            {
                str = ConvertHelper.ByteToHex(ConvertHelper.StringToByte(input));
            }
            catch
            {
            }

            return str;
        }

        public static string SubHex(string input, int startIndex, int count)
        {
            string str = "";
            input = Regex.Replace(input, @"\s", "");
            if (!(string.IsNullOrEmpty(input) || (input.Length < ((startIndex + count) * 2))))
            {
                str = input.Substring(startIndex * 2, count * 2);
            }

            return str;
        }

        public static string SubString(string input, int startIndex, int length)
        {
            string str = "";
            if (!(string.IsNullOrEmpty(input) || (input.Length < (startIndex + length))))
            {
                str = input.Substring(startIndex, length);
            }

            return str;
        }


        #region 单位换算
        /// <summary>
        /// 获取污染因子分子量
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        private static decimal GetMolecular(string pCode)
        {
            decimal num = -1M;
            string str = pCode.ToUpper();
            if (str == null)
            {
                return num;
            }
            if (!(str == "CO"))
            {
                if (str != "NO")
                {
                    if (str == "NO2")
                    {
                        return 46M;
                    }
                    if (str == "NOX")
                    {
                        return 46M;
                    }
                    if (str == "O3")
                    {
                        return 48M;
                    }
                    if (str != "SO2")
                    {
                        return num;
                    }
                    return 64M;
                }
            }
            else
            {
                return 28M;
            }
            return 30M;
        }

        public static decimal UgToPPB(string pCode, object value)
        {
            decimal d = (decimal.Parse(value.ToString()) * 22.4M) / GetMolecular(pCode);
            if (d < 0M)
            {
                d -= 0.05M;
            }
            else
            {
                d += 0.05M;
            }
            return Math.Round(d, 1);
        }

        public static decimal UgToPPM(string pCode, object value)
        {
            decimal d = ((decimal.Parse(value.ToString()) * 22.4M) / 1000M) / GetMolecular(pCode);
            if (d < 0M)
            {
                d -= 0.00005M;
            }
            else
            {
                d += 0.00005M;
            }
            return Math.Round(d, 4);
        }

        public static decimal MgToPPB(string pCode, object value)
        {
            decimal d = ((decimal.Parse(value.ToString()) * 22.4M) * 1000M) / GetMolecular(pCode);
            if (d < 0M)
            {
                d -= 0.05M;
            }
            else
            {
                d += 0.05M;
            }
            return Math.Round(d, 1);
        }

        public static decimal MgToPPM(string pCode, object value)
        {
            decimal d = (decimal.Parse(value.ToString()) * 22.4M) / GetMolecular(pCode);
            if (d < 0M)
            {
                d -= 0.00005M;
            }
            else
            {
                d += 0.00005M;
            }
            return Math.Round(d, 4);
        }

        public static decimal PPBToMg(string pCode, object value)
        {
            decimal d = ((decimal.Parse(value.ToString()) * GetMolecular(pCode)) / 22.4M) / 1000M;
            if (d < 0M)
            {
                d -= 0.00005M;
            }
            else
            {
                d += 0.00005M;
            }
            return Math.Round(d, 4);
        }

        public static decimal PPBToUg(string pCode, object value)
        {
            decimal d = (decimal.Parse(value.ToString()) * GetMolecular(pCode)) / 22.4M;
            if (d < 0M)
            {
                d -= 0.05M;
            }
            else
            {
                d += 0.05M;
            }
            return Math.Round(d, 1);
        }

        public static decimal PPMToMg(string pCode, object value)
        {
            decimal d = (decimal.Parse(value.ToString()) * GetMolecular(pCode)) / 22.4M;
            if (d < 0M)
            {
                d -= 0.00005M;
            }
            else
            {
                d += 0.00005M;
            }
            return Math.Round(d, 4);
        }

        public static decimal PPMToUg(string pCode, object value)
        {
            decimal d = ((decimal.Parse(value.ToString()) * GetMolecular(pCode)) / 22.4M) * 1000M;
            if (d < 0M)
            {
                d -= 0.05M;
            }
            else
            {
                d += 0.05M;
            }
            return Math.Round(d, 1);
        }
        #endregion


    }
}
