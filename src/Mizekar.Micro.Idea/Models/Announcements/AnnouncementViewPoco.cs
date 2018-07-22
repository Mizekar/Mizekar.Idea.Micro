using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Announcements
{
    public class AnnouncementViewPoco
    {
        public Guid Id { get; set; }
        public AnnouncementPoco Announcement { get; set; }
        /// <summary>
        /// تعداد ایده های مرتبط
        /// </summary>
        public long RelatedIdeasCount { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
