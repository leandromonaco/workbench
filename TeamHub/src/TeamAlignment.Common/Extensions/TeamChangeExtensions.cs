using TeamAlignment.Core.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class TeamChangeExtensions
    {
        public static List<DateTime> GetDates(this TeamChange teamChange)
        {
            var dates = new List<DateTime>();
            DateTime lastDay = DateTime.Now.AddMonths(3);
            if (teamChange.LastDay.HasValue)
            {
                lastDay = teamChange.LastDay.Value;
            }
            for (var dt = teamChange.FirstDay; dt <= lastDay; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            return dates.Where(d => !d.DayOfWeek.Equals(DayOfWeek.Saturday) && !d.DayOfWeek.Equals(DayOfWeek.Sunday)).ToList();
        }
    }
}
