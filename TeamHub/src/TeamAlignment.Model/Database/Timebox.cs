using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Timebox
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string WorkItemId { get; set; }
        public byte TimeboxCategoryId { get; set; }
        public int Hours { get; set; }

        public virtual Team Team { get; set; }
        public virtual TimeboxType TimeboxCategory { get; set; }
    }
}
