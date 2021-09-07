using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Timezone
    {
        public Timezone()
        {
            Locations = new HashSet<Location>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
