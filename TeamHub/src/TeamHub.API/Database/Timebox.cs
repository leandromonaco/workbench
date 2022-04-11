using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Timebox
    {
        public Guid Id { get; set; }
        public Guid? SprintId { get; set; }
        public Guid TeamId { get; set; }
        public byte TimeboxTypeId { get; set; }
        public int Hours { get; set; }

        public virtual Sprint? Sprint { get; set; }
        public virtual Team Team { get; set; } = null!;
        public virtual TimeboxType TimeboxType { get; set; } = null!;
    }
}
