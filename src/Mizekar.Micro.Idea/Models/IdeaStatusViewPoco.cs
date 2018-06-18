using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// نمایش وضعیت ایده
    /// </summary>
    public class IdeaStatusViewPoco
    {
        public Guid Id { get; set; }
        public IdeaStatusPoco IdeaStatus { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}