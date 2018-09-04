using System.Collections.Generic;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class IdeaAssessmentOptionSet : BusinessBaseEntity
    {
        public IdeaAssessmentOptionSet()
        {
            IdeaAssessmentOptionSetItems = new HashSet<IdeaAssessmentOptionSetItem>();
        }

        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public bool IsMultiSelect { get; set; }
        public bool IsSystemField { get; set; }

        public virtual ICollection<IdeaAssessmentOptionSetItem> IdeaAssessmentOptionSetItems { get; set; }
    }
}
