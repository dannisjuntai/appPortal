using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Utilities
{
    public static class StringExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string value)
        {
            return System.Text.Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetString(this byte[] value)
        {
            return System.Text.Encoding.UTF8.GetString(value); ;
        }

        /// <summary>
        /// Converts a DateTime into a (JavaScript parsable) Int64.
        /// </summary>
        /// <param name="from">The DateTime to convert from</param>
        /// <returns>An integer value representing the number of milliseconds since 1 January 1970 00:00:00 UTC.</returns>
        public static long Convert(this DateTime from)
        {
            DateTime initDt = new DateTime(1970, 1, 1);

            return System.Convert.ToInt64((from - initDt).TotalMilliseconds);
        }

        /// <summary>
        /// Converts a (JavaScript parsable) Int64 into a DateTime.
        /// </summary>
        /// <param name="from">An integer value representing the number of milliseconds since 1 January 1970 00:00:00 UTC.</param>
        /// <returns>The date as a DateTime</returns>
        public static DateTime Convert(this long from)
        {
            DateTime initDt = new DateTime(1970, 1, 1);
            return initDt.AddMilliseconds(from);
        }
        /// <summary>
        /// 連線狀態
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCurLinkStaName(this byte value)
        {
            switch (value)
            {
                case 0:
                    return "斷線";
                case 1:
                    return "通訊正常";
                case 2:
                    return "斷線";
                default:
                    return "斷線";
            }
        }
        /// <summary>
        /// 連線狀態
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetCurLinkSta(this byte value)
        {
            if (value == 1)
            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// 取得告警資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMaintainName(this byte value)
        {
            switch (value)
            {
                case 0:
                    return "正常";
                case 1:
                    return "保養";
                default:
                    return "保養";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetMaintain(this byte value)
        {
            if (value >= 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取得低值 flag
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetLowAlarmFlag(this byte? value)
        {
            if (value.HasValue == false)
            {
                return false;
            }
            if (value == 0)
            {
                return false;
            }
            if (value == 2)
            {
                return true;
            }
            if (value == 3)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取得高值 flag
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetUpAlarmFlag(this byte? value)
        {
            if (value.HasValue == false)
            {
                return false;
            }
            if (value == 0)
            {
                return false;
            }
            if (value == 1)
            {
                return true;
            }
            if (value == 3)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCurSubStaName(this byte? value)
        {
            if (value.HasValue == false)
            {
                return "空值";
            }
            switch (value)
            {
                case 0:
                    return "無";
                case 1:
                    return "正常";
                case 2:
                    return "超值";
                case 3:
                    return "低值";
                default:
                    return "其他";
            }
        }
        /// <summary>
        /// 取得Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNullString(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetStartTime(this DateTime date)
        {
            if (date == null)
            {
                DateTime now = new DateTime();
                return new DateTime(now.Year, now.Month, now.Day, 00, 00, 00);
            }
            else
            {
                DateTime dt = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetEndTime(this DateTime date)
        {
            if (date == null)
            {
                DateTime now = new DateTime();
                return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            }
            else
            {
                DateTime dt = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                return dt;
            }
        }
        /// <summary>
        /// Converts a DateTime to a javascript timestamp.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double ToJavascriptTimestamp(this DateTime input)
        {
            DateTime d1 = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime d2 = input;
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);


            return ts.TotalMilliseconds;
        }
        public static DateTime ToDateTime(this string date)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("zh-TW", true);
            return DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", culture);
        }
    }
}
