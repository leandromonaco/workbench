using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class Specialization
    {
        public Specialization()
        {
            Employee = new HashSet<Employee>();
        }

        public byte Id { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
