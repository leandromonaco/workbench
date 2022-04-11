using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Sprint
    {
        public Sprint()
        {
            Timeboxes = new HashSet<Timebox>();
        }

        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Timebox> Timeboxes { get; set; }
    }
}
