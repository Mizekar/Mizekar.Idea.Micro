using System;
using Mizekar.Core.Model.Api;
using Mizekar.Micro.Idea.Models.Announcements;
using Mizekar.Micro.Idea.Models.Services;

namespace Mizekar.Micro.Idea.Models
{
    public class IdeaViewPoco
    {
        public Guid Id { get; set; }
        public IdeaPoco Idea { get; set; }
        public IdeaStatusPoco IdeaStatus { get; set; }
        public AnnouncementPoco Announcement { get; set; }
        public ServicePoco Service { get; set; }
        public IdeaAdvancedFieldPoco AdvancedField { get; set; }
        public IdeaSocialStatisticPoco SocialStatistic { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}