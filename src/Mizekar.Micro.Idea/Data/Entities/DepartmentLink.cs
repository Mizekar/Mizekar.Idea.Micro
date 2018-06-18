using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class DepartmentLink : BusinessBaseEntity
    {
        public DepartmentLink()
        {

        }

        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        public Guid DepartmentId { get; set; }
    }
}