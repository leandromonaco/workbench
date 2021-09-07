using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class TimeboxType
    {
        public TimeboxType()
        {
            Timeboxes = new HashSet<Timebox>();
        }

        public byte Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Timebox> Timeboxes { get; set; }
    }
}
