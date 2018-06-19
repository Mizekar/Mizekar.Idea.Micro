using System;
using System.Collections.Generic;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// مورد گزینش
    /// </summary>
    public class IdeaOptionSetItem : BusinessBaseEntity
    {
        public IdeaOptionSetItem()
        {
            IdeaInfoOptionSetRelations = new HashSet<IdeaOptionSelection>();
        }
        public Guid IdeaOptionSetId { get; set; }
        public IdeaOptionSet IdeaOptionSet { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string HexColor { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSystemField { get; set; }

        public virtual ICollection<IdeaOptionSelection> IdeaInfoOptionSetRelations { get; set; }
    }
}
