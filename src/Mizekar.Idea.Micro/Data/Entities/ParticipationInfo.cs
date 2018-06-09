using System;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    /// <summary>
    /// مشارکت کنندگان
    /// </summary>
    public class ParticipationInfo : BusinessBaseEntity
    {
        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        /// <summary>
        /// کد کاربری داخل سیستم
        /// اگر عضو قبلی شبکه هست
        /// </summary>
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// نوع مشارکت
        /// </summary>
        public Guid PartnershipTypeId { get; set; }
        public virtual OptionSetItem PartnershipType { get; set; }
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
        public string Description { get; set; }
    }
}