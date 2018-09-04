using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class IdeaAssessmentOptionSelection : BusinessBaseEntity
    {
        public IdeaAssessmentOptionSelection()
        {

        }

        /// <summary>
        /// ایده ارزیابی شده
        /// </summary>
        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        public Guid IdeaAssessmentOptionSetId { get; set; }
        public virtual IdeaAssessmentOptionSet IdeaAssessmentOptionSet { get; set; }

        public Guid IdeaAssessmentOptionSetItemId { get; set; }
        public virtual IdeaAssessmentOptionSetItem IdeaAssessmentOptionSetItem { get; set; }
    }
}