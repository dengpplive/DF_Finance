using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class DateHelper
    {
        public static string ToString(DateTime date, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return date.ToString(format);
        }

        public static DateTime GetLastDayFor(DateTime dateTime)
        {
            var monthDays = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return Convert.ToDateTime(string.Format("{0}-{1}-{2} 23:59:59", dateTime.Year, dateTime.Month, monthDays));
        }

        public static DateTime GetFirstDayFor(DateTime dateTime)
        {
            return Convert.ToDateTime(dateTime.ToString("yyyy-MM-01 00:00:00"));
        }
    }
}
