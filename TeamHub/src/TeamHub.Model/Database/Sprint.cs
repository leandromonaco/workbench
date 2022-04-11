using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class Sprint
    {
        public Sprint()
        {
            Timebox = new HashSet<Timebox>();
        }

        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Timebox> Timebox { get; set; }
    }
}
