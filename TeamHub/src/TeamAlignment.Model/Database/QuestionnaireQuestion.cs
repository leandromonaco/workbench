using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class QuestionnaireQuestion
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public virtual QuestionnaireSection Section { get; set; }
    }
}
