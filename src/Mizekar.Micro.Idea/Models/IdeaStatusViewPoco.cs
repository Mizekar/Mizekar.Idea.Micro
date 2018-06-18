using System;

namespace Mizekar.Micro.Idea.Models
{
    /// <summary>
    /// نمایش وضعیت ایده
    /// </summary>
    public class IdeaStatusViewPoco
    {
        public Guid Id { get; set; }
        public IdeaStatusPoco IdeaStatus { get; set; }
    }
}