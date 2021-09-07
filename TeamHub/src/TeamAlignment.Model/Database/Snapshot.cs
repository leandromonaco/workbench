using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Snapshot
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime Timestamp { get; set; }
        public short Type { get; set; }
        public string Content { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Product Product { get; set; }
        public virtual Team Team { get; set; }
    }
}
