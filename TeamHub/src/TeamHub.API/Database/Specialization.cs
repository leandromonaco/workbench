using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Specialization
    {
        public Specialization()
        {
            Employees = new HashSet<Employee>();
        }

        public byte Id { get; set; }
        public string Description { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
