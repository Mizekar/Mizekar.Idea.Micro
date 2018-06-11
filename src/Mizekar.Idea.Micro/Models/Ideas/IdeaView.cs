using System;
using System.Collections.Generic;

namespace Mizekar.Idea.Micro.Models.Ideas
{
    public class IdeaView : BusinessBaseModel
    {
        public string Slug { get; set; }
        public bool IsDraft { get; set; }
        public int Status { get; set; }

        #region ------- طرح ایده -----------

        /// <summary>
        /// Idea Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Idea Summary Text
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// Idea Details Text
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// What problem does this Idea solve?
        /// </summary>
        public string Problem { get; set; }

        /// <summary>
        /// Priority by Owner of the idea
        /// </summary>
        public int Priority { get; set; }

        public bool IsPrivate { get; set; }

        #endregion

        #region ------------ ویژگی ها ---------------

        public List<Guid> StrategyLinks { get; set; }
        public List<Guid> DepartmentLinks { get; set; }
        public List<Guid> SubjectLinks { get; set; }
        public List<Guid> ScopeLinks { get; set; }

        #endregion

        #region ------- تفصیل -----------

        /// <summary>
        /// مقدمه
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// ثمرات و دستاورد
        /// </summary>
        public string Achievement { get; set; }
        /// <summary>
        /// لزوم اجرای طرح
        /// </summary>
        public string Necessity { get; set; }

        #endregion

        #region ------- مخاطب -----------

        /// <summary>
        /// پیش بینی تعداد مخاطب
        /// </summary>
        public int ContactCount { get; set; }

        #endregion

        #region ------- مستندات -----------

        /// <summary>
        /// آیا قبلا اجرا شده است؟
        /// </summary>
        public bool ImplementedInThePast { get; set; }
        public string ImplementedInThePastDesc { get; set; }
        //public virtual ICollection<ImplementedPastInfo> ImplementedPastInfos { get; set; }

        #endregion

        #region -------- مشارکت ----------------

        //public virtual ICollection<ParticipationInfo> ParticipationInfos { get; set; }

        #endregion

        #region -------- عملیات ----------------

        //public virtual ICollection<OperationalPhase> OperationalPhases { get; set; }
        //public virtual ICollection<RequirementEquipments> RequirementEquipmentses { get; set; }

        #endregion

        //public virtual ICollection<IdeaAtachement> IdeaAtachements { get; set; }
        //public virtual ICollection<IdeaInfoOptionSetRelation> IdeaInfoOptionSetRelations { get; set; }
    }
}