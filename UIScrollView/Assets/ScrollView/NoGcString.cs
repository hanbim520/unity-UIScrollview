/**
 * ClassName: NoGcString.cs
 * 
 * @author NavyZhang
 * @date 2018-01-30 上午11:15:43
 * @version 1.0
 * 
 * 
 * ............................................
 *                       _oo0oo_
 *                      o8888888o
 *                      88" . "88
 *                      (| -_- |)
 *                      0\  =  /0
 *                    ___/`---'\___
 *                  .' \\|     |// '.
 *                 / \\|||  :  |||// \
 *                / _||||| -卍-|||||- \
 *               |   | \\\  -  /// |   |
 *               | \_|  ''\---/''  |_/ |
 *               \  .-\__  '-'  ___/-. /
 *             ___'. .'  /--.--\  `. .'___
 *          ."" '<  `.___\_<|>_/___.' >' "".
 *         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
 *         \  \ `_.   \_ __\ /__ _/   .-` /  /
 *     =====`-.____`.___ \_____/___.-`___.-'=====
 *                       `=---='
 *                       
 *..................佛祖开光 ,永无BUG...................
 * 
 */
using System;
using System.Text;

namespace Gc
{

    public class NoGcString
    {

        private string string_base;
        private StringBuilder string_builder;
        private char[] int_parser = new char[11];

        public NoGcString(int capacity)
        {
            string_builder = new StringBuilder(capacity, capacity*10);
            string_base = (string)string_builder.GetType().GetField(
                "_str",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance).GetValue(string_builder);
        }


        //之所以会有这两种API，原因是文本显示控件的值是引用方式进来的，此处类型相同使用的内存是同一块，如果不分配一块新的内存，则会改变所有值。所以显示的时候就采用这种。
        public string GetValueForVisual()
        {
            return string_builder.ToString();
        }

        //此处api可用于整形做字符串key的时候的转换，一次性的，不可用于显示，除非不采用公用对象
        public string GetValueForNotVisual()
        {
            return string_base;
        }

        private int i;

        public void Clear()
        {
            string_builder.Length = 0;
            for (i = 0; i < string_builder.Capacity; i++)
            {
                string_builder.Append('\0');
            }
            string_builder.Length = 0;
        }

        //public void SetText(ref Text text)
        //{
        //    text.text = "";
        //    text.text = string_base;
        //    text.cachedTextGenerator.Invalidate();
        //}

        public void Append(string value)
        {
            string_builder.Append(value);
        }

        public void AppendFormat(string format,params object[] param)
        {
            string_builder.AppendFormat(format, param);
        }

        int count;

        public void Append(int value)
        {
            if (value >= 0)
            {
                count = ToCharArray((uint)value, int_parser, 0);
            }
            else
            {
                int_parser[0] = '-';
                count = ToCharArray((uint)-value, int_parser, 1) + 1;
            }
            for (i = 0; i < count; i++)
            {
                string_builder.Append(int_parser[i]);
            }
        }

        public void Append(uint value)
        {
            if (value >= 0)
            {
                count = ToCharArray((uint)value, int_parser, 0);
            }
            else
            {
                int_parser[0] = '-';
                count = ToCharArray((uint)-value, int_parser, 1) + 1;
            }
            for (i = 0; i < count; i++)
            {
                string_builder.Append(int_parser[i]);
            }
        }

        public void Append(long value)
        {
            if (value >= 0)
            {
                count = ToCharArray((ulong)value, int_parser, 0);
            }
            else
            {
                int_parser[0] = '-';
                count = ToCharArray((ulong)-value, int_parser, 1) + 1;
            }
            for (i = 0; i < count; i++)
            {
                string_builder.Append(int_parser[i]);
            }
        }

        public void Append(ulong value)
        {
            count = ToCharArray((ulong)value, int_parser, 0);
            for (i = 0; i < count; i++)
            {
                string_builder.Append(int_parser[i]);
            }
        }

        private int FloatMantissaSize(float data)
        {
            int len = 0;
            while (data - Math.Floor(data) > float.Epsilon)
            {
                len++;
                data *= 10;
            }

            return len;
        }

        public void Append(float value)
        {
            int integer = (int)value;
            int mantissa = (int)Math.Floor((value - integer) * Math.Pow(10, FloatMantissaSize(Math.Abs(value))));

            if (value >= 0)
            {
                count = ToCharArray((uint)integer, int_parser, 0);
                for (i = 0; i < count; i++)
                {
                    string_builder.Append(int_parser[i]);
                }
                count = ToCharArray((uint)mantissa, int_parser, 0);
                if (count > 0)
                    string_builder.Append(".");
                if (count >= 6)
                    count--;
                for (i = 0; i < count; i++)
                {
                    string_builder.Append(int_parser[i]);
                }
            }
            else
            {
                int_parser[0] = '-';
                count = ToCharArray((uint)-integer, int_parser, 1) + 1;
                for (i = 0; i < count; i++)
                {
                    string_builder.Append(int_parser[i]);
                }
                count = ToCharArray((uint)Math.Abs(mantissa), int_parser, 0);
                if (count > 0)
                    string_builder.Append(".");
                if (count >= 6)
                    count--;
                for (i = 0; i < count; i++)
                {
                    string_builder.Append(int_parser[i]);
                }
            }

        }

        private static readonly char[] number = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private static int ToCharArray(ulong value, char[] buffer, int bufferIndex)
        {
            if (value == 0)
            {
                buffer[bufferIndex] = '0';
                return 1;
            }
            int len = 1;
            for (ulong rem = value / 10; rem > 0; rem /= 10)
            {
                len++;
            }
            for (int i = len - 1; i >= 0; i--)
            {
                buffer[bufferIndex + i] = number[value % 10];
                value /= 10;
            }
            return len;
        }

        private static int ToCharArray(uint value, char[] buffer, int bufferIndex)
        {
            if (value == 0)
            {
                buffer[bufferIndex] = '0';
                return 1;
            }
            int len = 1;
            for (uint rem = value / 10; rem > 0; rem /= 10)
            {
                len++;
            }
            for (int i = len - 1; i >= 0; i--)
            {
                buffer[bufferIndex + i] = (char)('0' + (value % 10));
                value /= 10;
            }
            return len;
        }
    }

}