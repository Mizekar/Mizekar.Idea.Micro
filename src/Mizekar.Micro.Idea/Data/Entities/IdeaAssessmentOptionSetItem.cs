using System;
using System.Collections.Generic;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// مورد ارزیابی گزینش
    /// </summary>
    public class IdeaAssessmentOptionSetItem : BusinessBaseEntity
    {
        public IdeaAssessmentOptionSetItem()
        {
            IdeaAssessmentOptionSetRelations = new HashSet<IdeaAssessmentOptionSelection>();
        }
        public Guid IdeaAssessmentOptionSetId { get; set; }
        public IdeaAssessmentOptionSet IdeaAssessmentOptionSet { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public long Value { get; set; }
        public string HexColor { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSystemField { get; set; }

        public virtual ICollection<IdeaAssessmentOptionSelection> IdeaAssessmentOptionSetRelations { get; set; }
    }
}
