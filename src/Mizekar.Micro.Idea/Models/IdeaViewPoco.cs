using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models
{
    public class IdeaViewPoco
    {
        public Guid Id { get; set; }
        public IdeaPoco Idea { get; set; }
        public IdeaStatusPoco IdeaStatus { get; set; }
        public IdeaAdvancedFieldPoco AdvancedField { get; set; }
        public IdeaSocialStatisticPoco SocialStatistic { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}