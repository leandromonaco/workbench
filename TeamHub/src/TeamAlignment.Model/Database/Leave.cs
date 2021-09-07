using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Leave
    {
        public Guid Id { get; set; }
        public byte CategoryId { get; set; }
        public Guid TeamMemberId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPlanned { get; set; }

        public virtual LeaveType Category { get; set; }
        public virtual Employee TeamMember { get; set; }
    }
}
