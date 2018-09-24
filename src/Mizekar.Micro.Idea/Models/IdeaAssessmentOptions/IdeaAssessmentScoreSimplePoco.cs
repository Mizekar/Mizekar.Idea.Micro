using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    /// <summary>
    /// ارزیابی
    /// </summary>
    public class IdeaAssessmentScoreSimplePoco
    {
        /// <summary>
        /// مجموع امتیاز
        /// </summary>
        public long Score { get; set; }

        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}