using System;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class DepartmentLink : BusinessBaseEntity
    {
        public DepartmentLink()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public Guid DepartmentId { get; set; }
    }
}