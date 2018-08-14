using System;

namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// اطلاعات ایده
    /// </summary>
    public class IdeaPoco
    {
        public IdeaPoco()
        {
            IsDraft = false;
        }

        public string ImageId { get; set; }

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
        /// <summary>
        /// فراخوان مرتبط با ایده
        /// </summary>
        public Guid? AnnouncementId { get; set; }
        /// <summary>
        /// خدمت مرتبط با ایده
        /// </summary>
        public Guid? ServiceId { get; set; }

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

    }
}