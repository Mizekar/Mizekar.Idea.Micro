using System;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    /// <summary>
    /// ارزیابی
    /// </summary>
    public class IdeaAssessmentScorePoco
    {
        /// <summary>
        /// ایده ارزیابی شده
        /// </summary>
        public Guid IdeaId { get; set; }

        /// <summary>
        /// مجموع امتیاز
        /// </summary>
        public long Score { get; set; }
    }
}
