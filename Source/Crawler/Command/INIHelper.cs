using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Command
{
    public class INIHelper
    {
        private static string iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "DeviceSet.ini";
        public INIHelper(string iniPath)
        {
            iniFilePath = iniPath;
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string section, byte[] retVal, int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSectionNames(byte[] retVal, int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static Dictionary<string, string> ReadSection(string section)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                byte[] retVal = new byte[0x8000];
                int count = GetPrivateProfileSection(section, retVal, retVal.GetUpperBound(0), iniFilePath);
                string[] strArray = Encoding.GetEncoding("GB2312").GetString(retVal, 0, count).Trim(new char[1]).Split(new char[1]);
                foreach (string str2 in strArray)
                {
                    string[] strArray2 = str2.Split(new char[] { '=' });
                    dictionary[strArray2[0]] = strArray2[1];
                }
            }
            catch (Exception exception)
            {

            }
            return dictionary;
        }

        public static string[] ReadSectionNames()
        {
            byte[] retVal = new byte[0x400];
            int count = GetPrivateProfileSectionNames(retVal, retVal.Length, iniFilePath);
            return Encoding.GetEncoding("GB2312").GetString(retVal, 0, count).Trim(new char[1]).Split(new char[1]);
        }

        public static int ReadValue(string section, string key, int defaultValue)
        {
            StringBuilder retVal = new StringBuilder(0xff);
            int num = GetPrivateProfileString(section, key, defaultValue.ToString(), retVal, 0xff, iniFilePath);
            try
            {
                return Convert.ToInt32(retVal.ToString());
            }
            catch (System.Exception ex)
            {
                return defaultValue;
            }

        }

        public static string ReadValue(string section, string key, string defaultValue)
        {
            StringBuilder retVal = new StringBuilder(0xFFFFFF);
            int num = GetPrivateProfileString(section, key, defaultValue, retVal, 0xFFFFFF, iniFilePath);
            return retVal.ToString();
        }
        public static bool ReadValue(string section, string key, bool dValue)
        {
            StringBuilder retVal = new StringBuilder(0xff);
            string defaultValue = "0";
            if (dValue)
            {
                defaultValue = "1";
            }

            int num = GetPrivateProfileString(section, key, defaultValue, retVal, 0xff, iniFilePath);
            try
            {
                if (retVal.ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return dValue;
                }


            }
            catch (System.Exception ex)
            {
                return dValue;
            }
        }

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string retVal, string filePath);
        public static void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, iniFilePath);
        }
    }
}
