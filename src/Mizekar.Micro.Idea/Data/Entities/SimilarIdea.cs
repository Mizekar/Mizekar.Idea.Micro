using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// ایده های مشابه که قبلا اجرا شده
    /// </summary>
    public class SimilarIdea : BusinessBaseEntity
    {
        public Guid IdeaId { get; set; }
        public virtual IdeaInfo Idea { get; set; }


        /// <summary>
        /// صاحب ایده یا ایده دهنده
        /// </summary>
        public string OwnerFullName { get; set; }
        /// <summary>
        /// عنوان ایده
        /// </summary>
        public string IdeaTitle { get; set; }
        /// <summary>
        /// نام سازمان اجرا کننده
        /// </summary>
        public string OrganizationName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }


        /// <summary>
        /// کشور
        /// </summary>
        public Guid CountryId { get; set; }
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
    }
}