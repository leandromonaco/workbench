using System;
using System.Collections.Generic;

namespace TeamHub.API.Database
{
    public partial class Transfer
    {
        public Guid Id { get; set; }
        public Guid FromTeamId { get; set; }
        public Guid? ToTeamId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Team FromTeam { get; set; } = null!;
        public virtual Team? ToTeam { get; set; }
    }
}
