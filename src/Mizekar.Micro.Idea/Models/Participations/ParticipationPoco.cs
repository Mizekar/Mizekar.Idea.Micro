using System;

namespace Mizekar.Micro.Idea.Models.Participations
{
    /// <summary>
    /// مشارکت کننده در ایده
    /// </summary>
    public class ParticipationPoco
    {
        public Guid IdeaId { get; set; }

        /// <summary>
        /// کد کاربری داخل سیستم
        /// اگر عضو قبلی شبکه هست
        /// </summary>
        public long? ParticipantUserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// نوع مشارکت
        /// </summary>
        public string PartnershipType { get; set; }
        /// <summary>
        /// شیوه مشارکت
        /// </summary>
        public string PartnershipStyle { get; set; }
        /// <summary>
        /// میزان مشارکت
        /// </summary>
        public string PartnershipRate { get; set; }
        /// <summary>
        /// حوزه مشارکت
        /// </summary>
        public string ScopeOfPartnership { get; set; }

        /// <summary>
        /// انتظارات
        /// </summary>
        public string Expectation { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
    }
}
