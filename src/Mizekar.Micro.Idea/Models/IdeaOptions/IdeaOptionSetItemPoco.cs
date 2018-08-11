using System;

namespace Mizekar.Micro.Idea.Models.IdeaOptions
{
    /// <summary>
    /// موارد گزینه ها
    /// </summary>
    public class IdeaOptionSetItemPoco
    {
        public Guid IdeaOptionSetId { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string HexColor { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSystemField { get; set; }
    }
}
