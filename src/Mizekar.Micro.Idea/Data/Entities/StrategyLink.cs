using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class StrategyLink : BusinessBaseEntity
    {
        public StrategyLink()
        {

        }

        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        public Guid StrategyId { get; set; }
    }
}