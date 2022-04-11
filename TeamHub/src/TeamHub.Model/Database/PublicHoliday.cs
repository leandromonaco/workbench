using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class PublicHoliday
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid? LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}
