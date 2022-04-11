using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class Employee
    {
        public Employee()
        {
            InverseReportsToNavigation = new HashSet<Employee>();
            Leave = new HashSet<Leave>();
            Transfer = new HashSet<Transfer>();
        }

        public Guid Id { get; set; }
        public byte SpecializationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte HoursPerDay { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ReportsTo { get; set; }

        public virtual Location Location { get; set; }
        public virtual Employee ReportsToNavigation { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; }
        public virtual ICollection<Leave> Leave { get; set; }
        public virtual ICollection<Transfer> Transfer { get; set; }
    }
}
