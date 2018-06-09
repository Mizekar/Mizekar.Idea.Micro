using System;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class StrategyLink : BusinessBaseEntity
    {
        public StrategyLink()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public Guid StrategyId { get; set; }
    }
}