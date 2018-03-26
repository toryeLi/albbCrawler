using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace Command
{
    public enum LogLevel { Error, Info, Warning }
    public class LogClass
    {
        public static void LogToFile(string Message)
        {
            LogToFile(LogLevel.Info, Message, DateTime.Now, DateTime.Now, "I");
        }
        public static void LogToFile(string Message, string filename)
        {
            LogToFile(LogLevel.Info, Message, DateTime.Now, DateTime.Now, filename);
        }

        public static void LogToFile(LogLevel _level, string Message)
        {
            LogToFile(_level, Message, DateTime.Now, DateTime.Now, "I");
        }
        public static void LogToFile(LogLevel _level, string Message, string filename)
        {
            LogToFile(_level, Message, DateTime.Now, DateTime.Now, filename);
        }

        private static string bl = "1";

        public static void LogToFile(LogLevel _level, string AsMessage, DateTime StartTime, DateTime EndTime, string AsType)
        {
            lock (bl)
            {
                try
                {
                    System.Console.WriteLine(AsMessage);
                    string AppPath = System.AppDomain.CurrentDomain.BaseDirectory;

                    ///判断Log文件夹是否存在，不存在则创建
                    if (!System.IO.Directory.Exists(AppPath + "\\Log"))
                    {
                        System.IO.Directory.CreateDirectory(AppPath + "\\Log");
                    }
                    ///判断月份文件夹是否存在
                    if (!System.IO.Directory.Exists(AppPath + "\\Log\\" + DateTime.Now.ToString("yyyyMM")))
                    {
                        System.IO.Directory.CreateDirectory(AppPath + "\\Log\\" + DateTime.Now.ToString("yyyyMM"));
                    }
                    //
                    string LogPath = AppPath + "\\Log\\" + DateTime.Now.ToString("yyyyMM");

                    string newfilename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    string newfilenameError = "Error_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    string newfilenameWarning = "Warning_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    TimeSpan ts = EndTime - StartTime;
                    int ic = ts.Minutes * 60 + ts.Seconds;

                    string sTextLine = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + AsMessage;
                    using (StreamWriter sw = new StreamWriter(LogPath + "\\" + newfilename, true))
                    {
                        // sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][" + _level.ToString() + "][共用时" + ic.ToString() + "秒][开始时间" + StartTime + "][结束时间:" + EndTime+ "][" + filename + "][" + Message + "]");
                        sw.WriteLine(sTextLine);
                        sw.Close();
                    }
                    if (_level == LogLevel.Error)
                    {
                        using (StreamWriter sw = new StreamWriter(LogPath + "\\" + newfilenameError, true))
                        {
                            //  sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][" + _level.ToString() + "][共用时" + ic.ToString() + "秒][开始时间" + StartTime + "][结束时间:" + EndTime+ "][" + filename + "][" + Message + "]");
                            sw.WriteLine(sTextLine);
                            sw.Close();
                        }
                    }
                    if (_level == LogLevel.Warning)
                    {
                        using (StreamWriter sw = new StreamWriter(LogPath + "\\" + newfilenameWarning, true))
                        {
                            // sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][" + _level.ToString() + "][共用时" + ic.ToString() + "秒][开始时间" + StartTime + "][结束时间:" + EndTime+ "][" + filename + "][" + Message + "]");
                            sw.WriteLine(sTextLine);
                            sw.Close();
                        }
                    }

                    //如果处理时间超过2分钟或者是出错信息，将错误日志保存到数据库中
                    if (ic > 120 || _level == LogLevel.Error)
                    {
                        string strSerializeJSON = "";// JsonConvert.SerializeObject(obj.parameterList);
                        //LogToDB(filename, StartTime, EndTime, ic, strSerializeJSON, Message, _level.ToString());
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
