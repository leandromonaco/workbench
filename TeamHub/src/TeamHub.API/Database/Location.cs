using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Location
    {
        public Location()
        {
            Employees = new HashSet<Employee>();
            PublicHolidays = new HashSet<PublicHoliday>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string TimeZone { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<PublicHoliday> PublicHolidays { get; set; }
    }
}
