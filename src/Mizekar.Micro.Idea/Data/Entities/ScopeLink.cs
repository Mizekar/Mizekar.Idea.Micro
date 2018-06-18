using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class ScopeLink : BusinessBaseEntity
    {
        public ScopeLink()
        {

        }

        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        public Guid ScopeId { get; set; }
    }
}