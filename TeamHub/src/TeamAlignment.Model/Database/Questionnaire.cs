using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Questionnaire
    {
        public Questionnaire()
        {
            QuestionnaireSections = new HashSet<QuestionnaireSection>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }

        public virtual QuestionnairePeriod QuestionnairePeriod { get; set; }
        public virtual ICollection<QuestionnaireSection> QuestionnaireSections { get; set; }
    }
}
