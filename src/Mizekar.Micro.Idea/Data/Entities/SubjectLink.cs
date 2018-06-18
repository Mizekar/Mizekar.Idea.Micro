using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class SubjectLink : BusinessBaseEntity
    {
        public SubjectLink()
        {

        }

        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        public Guid SubjectId { get; set; }
    }
}