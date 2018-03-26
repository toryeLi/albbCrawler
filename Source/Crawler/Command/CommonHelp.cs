using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
   public class CommonHelp
    {
        /// <summary>
        ///获取该参数的数据源
        ///设置参数的数据源方法为：下限+0.1||0.01||0.001...=上限
        ///之间的数
        /// </summary>
        /// <param name="upperValue"></param>
        /// <param name="lowerValue"></param>
        /// <param name="DigitNum"></param>
        /// <returns></returns>
        public static List<double> getValus(double upperValue, double lowerValue, int DigitNum)
        {
            List<double> values = new List<double>();
            switch (DigitNum)
            {
                case 0:
                    for (double i = lowerValue; i <= upperValue; i++)
                    {
                        values.Add(Math.Round(i, DigitNum));
                    }
                    break;
                case 1:
                    for (double i = lowerValue; i <= upperValue; i = i + 0.1)
                    {
                        values.Add(Math.Round(i, DigitNum));
                    }
                    break;
                case 2:
                    for (double i = lowerValue; i <= upperValue; i = i + 0.01)
                    {
                        values.Add(Math.Round(i, DigitNum));
                    }
                    break;
                case 3:
                    for (double i = lowerValue; i <= upperValue; i = i + 0.001)
                    {
                        values.Add(Math.Round(i, DigitNum));
                    }
                    break;
                default:
                    for (double i = lowerValue; i <= upperValue; i = i + 0.0001)
                    {
                        values.Add(Math.Round(i, DigitNum));
                    }
                    break;
            }
            return values;
        }
        /// <summary>
        /// 返回随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max) {
            Guid guid = Guid.NewGuid();
            string sGuid = guid.ToString();
            int seed = DateTime.Now.Millisecond;
            for (int i = 0; i < sGuid.Length; i++)
            {
                switch (sGuid[i])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                        seed = seed + 1;
                        break;
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                        seed = seed + 2;
                        break;
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                        seed = seed + 3;
                        break;
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                        seed = seed + 3;
                        break;
                    default:
                        seed = seed + 4;
                        break;
                }
            }
            Random random = new Random(seed);
            return random.Next(min, max);
        }
    }
}
