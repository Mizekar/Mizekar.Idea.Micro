using System;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    /// <summary>
    /// فاز عملیاتی
    /// </summary>
    public class OperationalPhase : BusinessBaseEntity
    {
        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public string Title { get; set; }
        public int TimeRequiredByDays { get; set; }
        public long MoneyRequired { get; set; }
        public string Description { get; set; }
    }
}