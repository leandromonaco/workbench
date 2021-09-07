using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class KeyResult
    {
        public Guid Id { get; set; }
        public Guid ObjectiveId { get; set; }
        public string Description { get; set; }
    }
}
