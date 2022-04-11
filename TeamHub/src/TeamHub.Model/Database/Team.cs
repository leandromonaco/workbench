using System;
using System.Collections.Generic;

namespace TeamHub.Model.Database
{
    public partial class Team
    {
        public Team()
        {
            Timebox = new HashSet<Timebox>();
            TransferFromTeam = new HashSet<Transfer>();
            TransferToTeam = new HashSet<Transfer>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Timebox> Timebox { get; set; }
        public virtual ICollection<Transfer> TransferFromTeam { get; set; }
        public virtual ICollection<Transfer> TransferToTeam { get; set; }
    }
}
