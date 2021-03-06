﻿using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// اطلاعات آماری شبکه اجتماعی مربوط به این ایده
    /// </summary>
    public class IdeaSocialStatistic : BusinessBaseEntity
    {
        public IdeaSocialStatistic()
        {

        }

        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        /// <summary>
        /// تعداد لایک
        /// </summary>
        public long LikeCount { get; set; }
        /// <summary>
        /// مجموع امتیاز
        /// </summary>
        public long ScoreSum { get; set; }
        /// <summary>
        /// تعداد نظر
        /// </summary>
        public long CommentCount { get; set; }
        /// <summary>
        /// تعداد نمایش
        /// </summary>
        public long ViewCount { get; set; }
    }
}