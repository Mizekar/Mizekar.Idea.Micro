using System;
using System.Collections.Generic;

namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// فیلدهای پیشرفته ایده
    /// </summary>
    public class IdeaAdvancedFieldPoco
    {
        #region ------------ ویژگی ها ---------------
        /// <summary>
        /// راهبردهای مرتبط سازمان
        /// </summary>
        public List<Guid> StrategyLinks { get; set; }
        /// <summary>
        /// واحدهای مرتبط سازمان
        /// </summary>
        public List<Guid> DepartmentLinks { get; set; }
        /// <summary>
        /// موضوعات مرتبط سازمان
        /// </summary>
        public List<Guid> SubjectLinks { get; set; }
        /// <summary>
        /// حوزه های مرتبط سازمان
        /// </summary>
        public List<Guid> ScopeLinks { get; set; }

        #endregion


        #region ------- تفصیل -----------

        /// <summary>
        /// Introduction
        /// مقدمه
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// Results and Achievements
        /// ثمرات و دستاورد
        /// </summary>
        public string Achievement { get; set; }
        /// <summary>
        /// Necessity to implement
        /// لزوم اجرای طرح
        /// </summary>
        public string Necessity { get; set; }
        /// <summary>
        /// Full Details
        /// شرح کامل ایده
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// What problem does this Idea solve?
        /// این ایده چه مشکلی را حل میکند
        /// </summary>
        public string Problem { get; set; }

        #endregion
    }
}