using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class QuestionnaireSection
    {
        public QuestionnaireSection()
        {
            QuestionnaireQuestions = new HashSet<QuestionnaireQuestion>();
        }

        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
        public short Order { get; set; }
        public string Description { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ICollection<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
    }
}
