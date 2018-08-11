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
        /// <summary>
        /// گزینه ها
        /// </summary>
        public List<Guid> OptionItemIds { get; set; }

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

        #region ------- موقعیت جغرافیایی -----------

        /// <summary>
        /// کشور
        /// </summary>
        public Guid? CountryId { get; set; }
        /// <summary>
        /// استان
        /// </summary>
        public Guid? StateId { get; set; }
        ///// <summary>
        ///// شهرستان
        ///// </summary>
        //public Guid? ProvinceId { get; set; }
        /// <summary>
        /// شهر
        /// </summary>
        public Guid? CityId { get; set; }
        /// <summary>
        /// روستا
        /// </summary>
        public Guid? VillageId { get; set; }
        
        #endregion
    }
}