using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class PublicHoliday
    {
        public PublicHoliday()
        {
            PublicHolidaysLocations = new HashSet<PublicHolidayLocation>();
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PublicHolidayLocation> PublicHolidaysLocations { get; set; }
    }
}
