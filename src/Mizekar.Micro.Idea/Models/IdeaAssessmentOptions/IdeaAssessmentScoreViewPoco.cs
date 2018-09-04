using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    public class IdeaAssessmentScoreViewPoco
    {
        public Guid Id { get; set; }
        public IdeaAssessmentScorePoco IdeaAssessmentScore { get; set; }
        public IdeaPoco Idea { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
