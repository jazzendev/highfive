using System;
using System.Globalization;

namespace HighFive.Core.Utility
{
    public enum TimeFormat
    {
        /// <summary>
        /// yyyy年MM月dd日 HH:mm:ss
        /// </summary>
        Long = 1,
        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        Short = 2,
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        SimpleLong = 3,
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        SimpleShort = 4,
        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        StickyLong = 5,
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        StickyShort = 6
    }

    public static class DatetimeHelper
    {
        public static string ChineseLocalTime(this DateTime time, TimeFormat format = TimeFormat.Long)
        {
            return GetTimeString(time, 8, format);
        }

        /// <summary>
        /// 秒转换成时分秒
        /// 例：70秒 => 00:01:10
        /// </summary>
        /// <param name="second">秒数</param>
        /// <returns>Hour:Min:Sec</returns>
        public static string SecondChange(this int? second)
        {
            if (second == null || second <= 0)
                return "00:00:00";
            if (second >= 86400)
                return "23:59:59";
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(second));
            return string.Concat(ts.Hours.ToString().PadLeft(2, '0'), ":", ts.Minutes.ToString().PadLeft(2, '0'), ":", ts.Seconds.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// UTC字符串转换成北京时间
        /// 例：2020-11-03T02:08:34.906Z => 2020-11-03 10:08:34
        /// </summary>
        /// <param name="time">UTC字符串</param>
        /// <param name="format">显示的格式枚举</param>
        /// <returns></returns>
        public static string UTCTimeStrToChina(this string time, int hourOffset = 0, TimeFormat format = TimeFormat.Long)
        {
            DateTime dtTime = Convert.ToDateTime(time, CultureInfo.CurrentCulture);
            return GetTimeString(dtTime, hourOffset, format);
        }

        public static string ChinaToUTCTime(this DateTime time, TimeFormat format = TimeFormat.Long)
        {
            return GetTimeString(time, -8, format);
        }

        public static string ToTimeString(this DateTime time, TimeFormat format = TimeFormat.Long)
        {
            return GetTimeString(time, 0, format);
        }

        private static string GetTimeString(DateTime time, int hourOffset, TimeFormat format)
        {
            time = time.AddHours(hourOffset);
            switch (format)
            {
                case TimeFormat.StickyLong:
                    return time.ToString("yyyyMMddHHmmss");
                case TimeFormat.StickyShort:
                    return time.ToString("yyyyMMdd");
                case TimeFormat.Short:
                    return time.ToString("yyyy年MM月dd日");
                case TimeFormat.SimpleLong:
                    return time.ToString("yyyy-MM-dd HH:mm:ss");
                case TimeFormat.SimpleShort:
                    return time.ToString("yyyy-MM-dd");
                case TimeFormat.Long:
                default:
                    return time.ToString("yyyy年MM月dd日 HH:mm:ss");
            }
        }
    }
}
