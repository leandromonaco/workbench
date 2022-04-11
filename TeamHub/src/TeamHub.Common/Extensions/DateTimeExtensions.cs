using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt)
        {
            int diff = (dt.DayOfWeek - DayOfWeek.Friday) * -1;
            return dt.AddDays(diff).Date;
        }

        static IEnumerable<DateTime> GetDaysBetween(DateTime start, DateTime end)
        {
            for (DateTime i = start; i <= end; i = i.AddDays(1))
            {
                yield return i;
            }
        }

        public static int GetIdealHours(DateTime fromDate, DateTime toDate)
        {
            var workingDays = GetDaysBetween(fromDate, toDate).Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);
            return workingDays * 8;
        }

        public static int GetSprintDayCount(this DateTime dt, DateTime sprintDateFrom, DateTime sprintDateTo)
        {
            Dictionary<int, DateTime> sprintDates = new Dictionary<int, DateTime>();
            int sprintDayCount = 1;
            DateTime date = sprintDateFrom;
            while (!date.Equals(sprintDateTo))
            {
                if (!date.DayOfWeek.Equals(DayOfWeek.Saturday) &&
                    !date.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    sprintDates.Add(sprintDayCount, date);
                    sprintDayCount++;
                }
                date = date.AddDays(1);
            }

            int count = sprintDates.Where(sd => sd.Value.Equals(dt)).FirstOrDefault().Key;
            return count;
        }

        public static DateTime AdjustTimeZone(this DateTime date, string sourceTimeZone, string targetTimeZone)
        {
            var sourceTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZone);
            var targetTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZone);
            var adjustedDate = TimeZoneInfo.ConvertTimeToUtc(date, sourceTimeZoneInfo);
            adjustedDate = TimeZoneInfo.ConvertTimeFromUtc(adjustedDate, targetTimeZoneInfo);
            return adjustedDate;
        }

        public static DateTime AdjustUTCTimeZone(this DateTime utcDate, string targetTimeZone)
        {
            var targetTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZone);
            var adjustedDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, targetTimeZoneInfo);
            return adjustedDate;
        }
    }
}
