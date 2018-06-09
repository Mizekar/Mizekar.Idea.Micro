using System;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class SubjectLink : BusinessBaseEntity
    {
        public SubjectLink()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public Guid SubjectId { get; set; }
    }
}