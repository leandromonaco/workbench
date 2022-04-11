using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Team
    {
        public Team()
        {
            Timeboxes = new HashSet<Timebox>();
            TransferFromTeams = new HashSet<Transfer>();
            TransferToTeams = new HashSet<Transfer>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Timebox> Timeboxes { get; set; }
        public virtual ICollection<Transfer> TransferFromTeams { get; set; }
        public virtual ICollection<Transfer> TransferToTeams { get; set; }
    }
}
