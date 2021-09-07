using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Team
    {
        public Team()
        {
            Milestones = new HashSet<Milestone>();
            Snapshots = new HashSet<Snapshot>();
            TeamChanges = new HashSet<TeamChange>();
            Timeboxes = new HashSet<Timebox>();
        }

        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public DateTime SprintPlanningCutOff { get; set; }
        public DateTime ReleasePlanningCutOff { get; set; }
        public bool IsDisplayed { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Milestone> Milestones { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; }
        public virtual ICollection<TeamChange> TeamChanges { get; set; }
        public virtual ICollection<Timebox> Timeboxes { get; set; }
    }
}
