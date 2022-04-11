using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Employee
    {
        public Employee()
        {
            InverseReportsToNavigation = new HashSet<Employee>();
            Leaves = new HashSet<Leave>();
            Transfers = new HashSet<Transfer>();
        }

        public Guid Id { get; set; }
        public byte SpecializationId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte HoursPerDay { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ReportsTo { get; set; }

        public virtual Location? Location { get; set; }
        public virtual Employee? ReportsToNavigation { get; set; }
        public virtual Specialization Specialization { get; set; } = null!;
        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
    }
}
