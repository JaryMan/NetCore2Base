using System;
using System.Globalization;
using System.Linq;

namespace MeiMing.Framework.Extensions.DateTime_
{
    /// <summary>
    /// 包含一组静态方法，这些方法是 <see cref="System.DateTime"/> 的扩展方法，使用时请添加对命名空间 <see cref="Framework.Extensions.DateTime_"/> 的引用。
    /// </summary>
    public static class Extensions
    {
        ///// <summary>
        ///// 返回与当前 <see cref="DateTime"/> 关联的属相信息。
        ///// </summary>
        ///// <param name="dateTime"><see cref="DateTime"/></param>
        ///// <returns>返回 <see cref="AnimalSign"/> 枚举值之一。</returns>
        //public static AnimalSign GetAnimalSign(this DateTime dateTime) { return (AnimalSign)(dateTime.Year % 12 + 1); }

        ///// <summary>
        ///// 返回与当前 <see cref="DateTime"/> 关联的星座信息。
        ///// </summary>
        ///// <param name="dateTime"><see cref="DateTime"/></param>
        ///// <returns>返回 <see cref="StarSign"/> 枚举值之一。</returns>
        //public static StarSign GetStarSign(this DateTime dateTime) { return Global.StarSignVictor.Descriptions.Where(desc => (desc.Value.StartMonth == dateTime.Month && dateTime.Day >= desc.Value.StartDay) || (desc.Value.EndMonth == dateTime.Month && dateTime.Day <= desc.Value.EndDay)).Select(desc => desc.Key).FirstOrDefault(); }

        /// <summary>
        /// 返回当前 <see cref="DateTime"/> 与 <see cref="DateTime.Now"/> 的时间差。
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <returns>返回 <see cref="TimeSpan"/>。</returns>
        public static TimeSpan GetDateDiffOfNow(this DateTime dateTime) { return DateTime.Now - dateTime; }

        /// <summary>
        /// 根据当前 <see cref="DateTime"/> 与指定的表示生日的 <see cref="DateTime"/> 返回年龄。
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <param name="birthday">指定表示生日的 <see cref="DateTime"/>。</param>
        /// <returns>返回 <see cref="Int32"/>。</returns>
        public static int CalculateAge(this DateTime dateTime, DateTime birthday)
        {
            var age = DateTime.Now.Year - birthday.Year;

            return DateTime.Now.Month < birthday.Month || (DateTime.Now.Month == birthday.Month && DateTime.Now.Day < birthday.Day) ? --age : age;
        }

        /// <summary>
        /// 返回当前日期的TimeStamp格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>返回 <see cref="Int32"/>。</returns>
        public static ulong ToTimeStamp(this DateTime dateTime)
        {
            if (Convert.ToDateTime("1970-1-1") >= dateTime)
            {
                return 0;
            }
            return Convert.ToUInt64((dateTime.AddHours(-8) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
        }

        /// <summary>
        /// 返回当前TimeStamp的日期格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this ulong timeStamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(Convert.ToDouble(timeStamp));
        }

        /// <summary>
        /// 返回当前TimeStamp的日期格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this UInt32 timeStamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(Convert.ToDouble(timeStamp));
        }

        /// <summary>
        /// 返回时间为所在年份中的第几个星期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetIso8601WeekOfYear(this DateTime time)
        {

            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }


        /// <summary>
        /// 计算两个日期的时间间隔
        /// </summary>
        /// <param name="dateTime1">第一个日期和时间</param>
        /// <param name="dateTime2">第二个日期和时间</param>
        /// <returns>时间间隔</returns>
        public static string DateDiff(this DateTime dateTime1, DateTime dateTime2)
        {
            var ts = new TimeSpan(dateTime1.Ticks).Subtract(new TimeSpan(dateTime2.Ticks));//.Duration();

            var strDateDiff = String.Empty;
            if (ts.Days != 0)
            {
                strDateDiff += ts.Days + "天";
            }
            if (ts.Hours != 0)
            {
                strDateDiff += Math.Abs(ts.Hours) + "小时";
            }
            if (ts.Minutes != 0)
            {
                strDateDiff += Math.Abs(ts.Minutes) + "分钟";
            }
            //if (ts.Seconds > 0)
            //{
            //    strDateDiff += ts.Seconds + "秒";
            //}
            return strDateDiff;
        }

    }
}