using System;
using System.Collections.Generic;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class OptionSetItem : BusinessBaseEntity
    {
        public OptionSetItem()
        {
            IdeaInfoOptionSetRelations = new HashSet<IdeaInfoOptionSetRelation>();
        }
        public Guid OptionSetId { get; set; }
        public OptionSet OptionSet { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string HexColor { get; set; }
        public bool IsDisable { get; set; }

        public virtual ICollection<IdeaInfoOptionSetRelation> IdeaInfoOptionSetRelations { get; set; }
    }
}
