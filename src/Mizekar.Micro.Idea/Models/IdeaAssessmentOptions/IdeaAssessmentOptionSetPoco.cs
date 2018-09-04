using System;

namespace Mizekar.Micro.Idea.Models.IdeaAssessmentOptions
{
    /// <summary>
    /// ارزیابی
    /// </summary>
    public class IdeaAssessmentOptionSetPoco
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public bool IsMultiSelect { get; set; }
        public bool IsSystemField { get; set; }
    }
}
