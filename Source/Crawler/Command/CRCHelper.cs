using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Command
{
   public class CRCHelper
    {
        private static byte[] ConvertToByteData(string data)
        {
            byte[] buffer = new byte[data.Length / 2];
            try
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    string str = data.Substring(i * 2, 2);
                    buffer[i] = Convert.ToByte(str, 0x10);
                }
            }
            catch
            {
            }
            return buffer;
        }

        private static byte[] CRC16(byte[] data, int crcPoly)
        {
            int num4 = 0xffff;
            for (int i = 0; i < data.Length; i++)
            {
                num4 ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    int num3 = num4 & 1;
                    num4 = num4 >> 1;
                    if (num3 > 0)
                    {
                        num4 ^= crcPoly;
                    }
                }
            }
            byte[] destinationArray = new byte[data.Length + 2];
            Array.Copy(data, 0, destinationArray, 0, data.Length);
            destinationArray[destinationArray.Length - 2] = (byte)(num4 & 0xff);
            destinationArray[destinationArray.Length - 1] = (byte)(num4 >> 8);
            return destinationArray;
        }

        public static string CRC16(string data, int crcPoly)
        {
            data = Regex.Replace(data, @"\s", "");
            if (string.IsNullOrEmpty(data))
            {
                return "";
            }
            return FormatData(CRC16(ConvertToByteData(data), crcPoly));
        }

        private static string FormatData(byte[] data)
        {
            string str = string.Empty;
            foreach (byte num in data)
            {
                str = str + string.Format("{0:X2} ", num);
            }
            return str;
        }
    }
}
