using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class IdeaInfo : BusinessBaseEntity
    {
        public IdeaInfo()
        {
            StrategyLinks = new HashSet<StrategyLink>();
            DepartmentLinks = new HashSet<DepartmentLink>();
            SubjectLinks = new HashSet<SubjectLink>();
            ScopeLinks = new HashSet<ScopeLink>();
            ImplementedPastInfos = new HashSet<ImplementedPastInfo>();
            IdeaAtachements = new HashSet<IdeaAtachement>();
            IdeaInfoOptionSetRelations = new HashSet<IdeaInfoOptionSetRelation>();
            ParticipationInfos = new HashSet<ParticipationInfo>();
            OperationalPhases = new HashSet<OperationalPhase>();
            RequirementEquipmentses = new HashSet<RequirementEquipments>();
        }

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

        public virtual ICollection<StrategyLink> StrategyLinks { get; set; }
        public virtual ICollection<DepartmentLink> DepartmentLinks { get; set; }
        public virtual ICollection<SubjectLink> SubjectLinks { get; set; }
        public virtual ICollection<ScopeLink> ScopeLinks { get; set; }

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
        public virtual ICollection<ImplementedPastInfo> ImplementedPastInfos { get; set; }

        #endregion

        #region -------- مشارکت ----------------

        public virtual ICollection<ParticipationInfo> ParticipationInfos { get; set; }

        #endregion

        #region -------- عملیات ----------------

        public virtual ICollection<OperationalPhase> OperationalPhases { get; set; }
        public virtual ICollection<RequirementEquipments> RequirementEquipmentses { get; set; }

        #endregion

        public virtual ICollection<IdeaAtachement> IdeaAtachements { get; set; }
        public virtual ICollection<IdeaInfoOptionSetRelation> IdeaInfoOptionSetRelations { get; set; }

    }

    public class IdeaInfoOptionSetRelation : BusinessBaseEntity
    {
        public IdeaInfoOptionSetRelation()
        {

        }

        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public Guid OptionSetId { get; set; }
        public virtual OptionSet OptionSet { get; set; }

        public Guid OptionSetItemId { get; set; }
        public virtual OptionSetItem OptionSetItem { get; set; }
    }
}
