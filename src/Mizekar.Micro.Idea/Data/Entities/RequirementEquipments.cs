using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// امکانات مورد نیاز
    /// </summary>
    public class RequirementEquipments : BusinessBaseEntity
    {
        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public string Title { get; set; }
        public int TimeRequiredByDays { get; set; }
        public long MoneyRequired { get; set; }
        public string Description { get; set; }
    }
}