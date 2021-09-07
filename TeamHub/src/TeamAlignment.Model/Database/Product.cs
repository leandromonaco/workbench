using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Product
    {
        public Product()
        {
            Snapshots = new HashSet<Snapshot>();
            Teams = new HashSet<Team>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ConfigurationId { get; set; }

        public virtual Setting Configuration { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
