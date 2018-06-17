using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class IdeaSocialStatistic : BusinessBaseEntity
    {
        public IdeaSocialStatistic()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public long LikeCount { get; set; }
        public long ScoreSum { get; set; }
        public long CommentCount { get; set; }
        public long ViewCount { get; set; }
    }
}