using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Employee
    {
        public Employee()
        {
            InverseReportsToNavigation = new HashSet<Employee>();
            Leaves = new HashSet<Leave>();
            Snapshots = new HashSet<Snapshot>();
            TeamChanges = new HashSet<TeamChange>();
        }

        public Guid Id { get; set; }
        public byte SpecializationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LoginUser { get; set; }
        public byte HoursPerDay { get; set; }
        public Guid? ReportsTo { get; set; }

        public virtual Employee ReportsToNavigation { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; }
        public virtual ICollection<TeamChange> TeamChanges { get; set; }
    }
}
