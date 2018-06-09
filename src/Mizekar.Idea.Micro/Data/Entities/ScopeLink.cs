using System;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class ScopeLink : BusinessBaseEntity
    {
        public ScopeLink()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public Guid ScopeId { get; set; }
    }
}