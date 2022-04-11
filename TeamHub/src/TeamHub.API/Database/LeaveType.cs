using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class LeaveType
    {
        public LeaveType()
        {
            Leaves = new HashSet<Leave>();
        }

        public byte Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<Leave> Leaves { get; set; }
    }
}
