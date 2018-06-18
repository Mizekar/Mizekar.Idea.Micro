using System;

namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// Idea Filter
    /// فیلتر ایده
    /// </summary>
    public class IdeaFilterOptionPoco
    {
        /// <summary>
        /// کلمات کلیدی برای جستجو
        /// </summary>
        public string[] Keywords { get; set; }
        /// <summary>
        /// کد ایده دهنده ها
        /// </summary>
        public long[] OwnerIds { get; set; }
        /// <summary>
        /// کد مشارکت کننده ها
        /// </summary>
        public long[] ParticipantIds { get; set; }
        /// <summary>
        /// کد وضعیت
        /// </summary>
        public Guid[] StatusIds { get; set; }
        /// <summary>
        /// کد راهبردها
        /// </summary>
        public Guid[] StrategyIds { get; set; }
        /// <summary>
        /// کد حوزه ها
        /// </summary>
        public Guid[] ScopeIds { get; set; }
        /// <summary>
        /// کد موضوعات
        /// </summary>
        public Guid[] SubjectIds { get; set; }
        /// <summary>
        /// کد واحدها
        /// </summary>
        public Guid[] DepartmentIds { get; set; }
    }
}