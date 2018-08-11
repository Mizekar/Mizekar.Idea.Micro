using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// اطلاعات پایه ایده
    /// </summary>
    public class IdeaInfo : BusinessBaseEntity
    {
        public IdeaInfo()
        {
            StrategyLinks = new HashSet<StrategyLink>();
            DepartmentLinks = new HashSet<DepartmentLink>();
            SubjectLinks = new HashSet<SubjectLink>();
            ScopeLinks = new HashSet<ScopeLink>();
            SimilarIdeas = new HashSet<SimilarIdea>();
            IdeaOptionSelections = new HashSet<IdeaOptionSelection>();
            Participations = new HashSet<Participation>();
            OperationalPhases = new HashSet<OperationalPhase>();
            Requirements = new HashSet<Requirement>();
            SocialStatistics = new HashSet<IdeaSocialStatistic>();
        }

        /// <summary>
        /// آدرس کوتاه
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// User Id of Owner
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// Is Deaft Status
        /// آیا پیش نویس هست؟
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Idea Status
        /// وضعیت ایده
        /// </summary>
        public Guid IdeaStatusId { get; set; }
        public virtual IdeaStatus IdeaStatus { get; set; }

        public Guid? AnnouncementId { get; set; }
        public virtual Announcement Announcement { get; set; }

        public Guid? ServiceId { get; set; }
        public virtual Service Service { get; set; }

        #region ------- طرح ایده -----------

        /// <summary>
        /// Idea Title
        /// موضوع ایده
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Idea Text
        /// متن ایده
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Priority by Owner of the idea
        /// اولویت ایده به نظر صاحب ایده
        /// </summary>
        public int? PriorityByOwner { get; set; }

        #endregion

        #region ------------ ویژگی ها ---------------

        /// <summary>
        /// راهبردهای مرتبط سازمان
        /// </summary>
        public virtual ICollection<StrategyLink> StrategyLinks { get; set; }

        /// <summary>
        /// واحدهای مرتبط سازمان
        /// </summary>
        public virtual ICollection<DepartmentLink> DepartmentLinks { get; set; }

        /// <summary>
        /// موضوعات مرتبط سازمان
        /// </summary>
        public virtual ICollection<SubjectLink> SubjectLinks { get; set; }

        /// <summary>
        /// حوزه های مرتبط سازمان
        /// </summary>
        public virtual ICollection<ScopeLink> ScopeLinks { get; set; }

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


        /// <summary>
        /// ایده های مشابه که قبلا اجرا شده
        /// </summary>
        public virtual ICollection<SimilarIdea> SimilarIdeas { get; set; }

        /// <summary>
        /// مشارکت کنندگان مورد نیاز
        /// </summary>
        public virtual ICollection<Participation> Participations { get; set; }

        /// <summary>
        /// فازهای اجرایی مورد نیاز
        /// </summary>
        public virtual ICollection<OperationalPhase> OperationalPhases { get; set; }

        /// <summary>
        /// تجهیزات و امکانات مورد نیاز برای اجرا
        /// </summary>
        public virtual ICollection<Requirement> Requirements { get; set; }

        /// <summary>
        /// سایر تنظیمات
        /// </summary>
        public virtual ICollection<IdeaOptionSelection> IdeaOptionSelections { get; set; }

        /// <summary>
        /// اطلاعات آماری
        /// </summary>
        public virtual ICollection<IdeaSocialStatistic> SocialStatistics { get; set; }


    }
}
