using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class PublicHolidayLocation
    {
        public Guid LocationId { get; set; }
        public Guid PublicHolidayId { get; set; }

        public virtual Location Location { get; set; }
        public virtual PublicHoliday PublicHoliday { get; set; }
    }
}
