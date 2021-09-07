using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class LeaveType
    {
        public LeaveType()
        {
            Leaves = new HashSet<Leave>();
        }

        public byte Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Leave> Leaves { get; set; }
    }
}
