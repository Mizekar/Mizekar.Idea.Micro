using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    public class IdeaAssessmentOptionSetViewPoco
    {
        public Guid Id { get; set; }
        public IdeaAssessmentOptionSetPoco IdeaAssessmentOptionSet { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
