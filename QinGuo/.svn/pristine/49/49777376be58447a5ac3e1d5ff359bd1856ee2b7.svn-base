using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Common
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            //var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            //return Convert.ToInt64((dateTime - start).TotalSeconds);

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(dateTime.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return Convert.ToInt32(timeStamp);

        }

      
    }
}
