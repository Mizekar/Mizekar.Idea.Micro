using System;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    /// <summary>
    /// موارد گزینه ها
    /// </summary>
    public class IdeaAssessmentOptionSetItemPoco
    {
        public Guid IdeaAssessmentOptionSetId { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public long Value { get; set; }
        public string HexColor { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSystemField { get; set; }
    }
}
