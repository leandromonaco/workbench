using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class QuestionnairePeriod
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
