using TeamAlignment.Core.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class LeaveExtensions
    {
        private static TeamAlignmentContext _dataContext = new TeamAlignmentContext();

        public static List<DateTime> GetDates(this Leave timeOff)
        {
            var dates = new List<DateTime>();
            for (var dt = timeOff.StartDate; dt <= timeOff.EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            return dates.Where(d => !d.DayOfWeek.Equals(DayOfWeek.Saturday) && !d.DayOfWeek.Equals(DayOfWeek.Sunday)).ToList();
        }
    }
}
