using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class Location
    {
        public Location()
        {
            Employee = new HashSet<Employee>();
            PublicHoliday = new HashSet<PublicHoliday>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TimeZone { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<PublicHoliday> PublicHoliday { get; set; }
    }
}
