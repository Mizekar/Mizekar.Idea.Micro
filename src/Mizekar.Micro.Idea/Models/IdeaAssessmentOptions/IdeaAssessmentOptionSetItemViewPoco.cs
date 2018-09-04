using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    public class IdeaAssessmentOptionSetItemViewPoco
    {
        public Guid Id { get; set; }
        public IdeaAssessmentOptionSetItemPoco IdeaAssessmentOptionSetItem { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
