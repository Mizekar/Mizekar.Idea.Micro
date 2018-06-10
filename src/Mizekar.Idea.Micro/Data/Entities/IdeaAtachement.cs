using System;
using Mizekar.Core.Data;
using Newtonsoft.Json;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class IdeaAtachement : BusinessBaseEntity
    {
        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public AtachementType AtachementType { get; set; }
        public Guid FileId { get; set; }
    }
}