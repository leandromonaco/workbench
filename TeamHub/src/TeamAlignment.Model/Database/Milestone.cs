using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Milestone
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public virtual Team Team { get; set; }
    }
}
