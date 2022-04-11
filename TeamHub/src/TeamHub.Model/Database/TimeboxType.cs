using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class TimeboxType
    {
        public TimeboxType()
        {
            Timebox = new HashSet<Timebox>();
        }

        public byte Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Timebox> Timebox { get; set; }
    }
}
