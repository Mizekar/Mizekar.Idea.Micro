using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class IdeaInfoOptionSetRelation : BusinessBaseEntity
    {
        public IdeaInfoOptionSetRelation()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public Guid OptionSetId { get; set; }
        public virtual OptionSet OptionSet { get; set; }

        public Guid OptionSetItemId { get; set; }
        public virtual OptionSetItem OptionSetItem { get; set; }
    }
}