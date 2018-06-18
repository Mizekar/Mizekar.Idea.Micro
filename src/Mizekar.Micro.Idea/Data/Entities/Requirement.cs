using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// امکانات مورد نیاز
    /// </summary>
    public class Requirement : BusinessBaseEntity
    {
        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }

        /// <summary>
        /// عنوان امکانات
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// ترتیب
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// زمان مورد نیاز بر اساس روز
        /// </summary>
        public int TimeRequiredByDays { get; set; }
        /// <summary>
        /// اعتبار مورد نیاز
        /// </summary>
        public long MoneyRequired { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
    }
}