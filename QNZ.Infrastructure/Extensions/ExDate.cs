using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIG.Infrastructure.Extensions
{
     public static class ExDate
     {
        private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)).Ticks;

        /// <summary>
        /// 将星期几转化为日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }
        /// <summary>
        /// Get first day in this month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToStartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// get last day in this month 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToEndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获取MS SQL数据支持的最少日期
        /// </summary>
        public static DateTime MinDateTime
        {
            get { return new DateTime(1900, 1, 1); }
        }

        /// <summary>
        /// 格式化日期(24小时制)，返回格式如：2012-03-02
        /// </summary>
        /// <param name="dt">需要格式的日期</param>
        /// <returns>格式化后的日期</returns>
        public static string Format(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化日期(24小时制)，返回格式如：2012-03-02 09:10:11
        /// </summary>
        /// <param name="dt">需要格式的日期</param>
        /// <returns>格式化后的日期</returns>
        public static string FormatL(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化日期(24小时制)，返回格式如：2012年03月02日
        /// </summary>
        /// <param name="dt">需要格式的日期</param>
        /// <returns>格式化后的日期</returns>
        public static string FormatD(this DateTime dt)
        {
            return dt.ToString("yyyy年MM月dd日");
        }

        /// <summary>
        /// 格式化日期(24小时制)，返回格式如：2012年03月02日 09时10分11秒
        /// </summary>
        /// <param name="dt">需要格式的日期</param>
        /// <returns>格式化后的日期</returns>
        public static string FormatLd(this DateTime dt)
        {
            return dt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
        }
        /// <summary>
        /// 得到一年中的某周的起始日和截止日
        /// 年 nYear
        /// 周数 nNumWeek
        /// 周始 out dtWeekStart
        /// 周终 out dtWeekeEnd
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="nNumWeek"></param>
        /// <param name="dtWeekStart"></param>
        /// <param name="dtWeekeEnd"></param>
        public static void GetWeek(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            var dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Sunday);
            if (dtWeekStart < new DateTime(nYear, 1, 1))
                dtWeekStart = new DateTime(nYear, 1, 1);

            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
            if (dtWeekeEnd > new DateTime(nYear, 1, 1).AddYears(1))
                dtWeekeEnd = new DateTime(nYear, 1, 1).AddYears(1);
        }

        /**/

        /// <summary>
        /// 求某年有多少周
        /// 返回 int
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns>int</returns>
        public static int GetYearWeekCount(int strYear)
        {
            var fDt = DateTime.Parse(strYear + "-01-01");
            var k = Convert.ToInt32(fDt.DayOfWeek); //得到该年的第一天是周几 
            if (k == 1)
            {
                var countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                var countWeek = countDay / 7 + 1;
                return countWeek;
            }
            else
            {
                var countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                var countWeek = countDay / 7 + 2;
                return countWeek;
            }
        }

        /**/

        /// <summary>
        /// 求当前日期是一年的中第几周
        /// </summary>
        /// <param name="curDay"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime curDay)
        {
            var firstdayofweek = Convert.ToInt32(Convert.ToDateTime(curDay.Year + "-01-01 ").DayOfWeek);

            var days = curDay.DayOfYear;
            var daysOutOneWeek = days - (7 - firstdayofweek);

            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            var weeks = daysOutOneWeek / 7;
            if (daysOutOneWeek % 7 != 0)
                weeks++;

            return weeks + 1;
        }


    }
}
