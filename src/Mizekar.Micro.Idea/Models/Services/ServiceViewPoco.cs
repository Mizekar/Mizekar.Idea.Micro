using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Services
{
    public class ServiceViewPoco
    {
        public Guid Id { get; set; }
        public ServicePoco Service { get; set; }
        /// <summary>
        /// تعداد ایده های مرتبط
        /// </summary>
        public long RelatedIdeasCount { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
