using System;

namespace Mizekar.Micro.Idea.Models.Announcements
{
    /// <summary>
    /// فراخوان
    /// </summary>
    public class AnnouncementPoco
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
