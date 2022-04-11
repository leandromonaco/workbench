using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class LeaveType
    {
        public LeaveType()
        {
            Leave = new HashSet<Leave>();
        }

        public byte Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Leave> Leave { get; set; }
    }
}
