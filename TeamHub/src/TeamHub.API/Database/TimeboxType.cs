using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class TimeboxType
    {
        public TimeboxType()
        {
            Timeboxes = new HashSet<Timebox>();
        }

        public byte Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<Timebox> Timeboxes { get; set; }
    }
}
