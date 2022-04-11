using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Leave
    {
        public Guid Id { get; set; }
        public byte LeaveTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPlanned { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual LeaveType LeaveType { get; set; } = null!;
    }
}
