using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class IdeaOptionSelection : BusinessBaseEntity
    {
        public IdeaOptionSelection()
        {

        }

        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        public Guid IdeaOptionSetId { get; set; }
        public virtual IdeaOptionSet IdeaOptionSet { get; set; }

        public Guid IdeaOptionSetItemId { get; set; }
        public virtual IdeaOptionSetItem IdeaOptionSetItem { get; set; }
    }
}