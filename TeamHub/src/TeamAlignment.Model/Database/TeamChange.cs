using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class TeamChange
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Guid TeamMemberId { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime? LastDay { get; set; }

        public virtual Team Team { get; set; }
        public virtual Employee TeamMember { get; set; }
    }
}
