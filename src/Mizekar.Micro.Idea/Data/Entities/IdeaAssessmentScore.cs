using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// مجموع امتیاز
    /// </summary>
    public class IdeaAssessmentScore : BusinessBaseEntity
    {
        public IdeaAssessmentScore()
        {

        }

        /// <summary>
        /// ایده ارزیابی شده
        /// </summary>
        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        /// <summary>
        /// مجموع امتیاز
        /// </summary>
        public long Score { get; set; }
    }
}