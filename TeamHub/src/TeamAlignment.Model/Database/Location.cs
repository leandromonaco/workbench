using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Location
    {
        public Location()
        {
            PublicHolidaysLocations = new HashSet<PublicHolidayLocation>();
            Teams = new HashSet<Team>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TimeZoneId { get; set; }

        public virtual Timezone TimeZone { get; set; }
        public virtual ICollection<PublicHolidayLocation> PublicHolidaysLocations { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
