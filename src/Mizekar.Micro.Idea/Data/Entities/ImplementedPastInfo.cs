using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    public class ImplementedPastInfo : BusinessBaseEntity
    {
        public Guid IdeaInfoId { get; set; }
        public virtual IdeaInfo IdeaInfo { get; set; }

        public string OwnerFullName { get; set; }
        public string IdeaTitle { get; set; }
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