using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// فراخوان
    /// </summary>
    public class Announcement : BusinessBaseEntity
    {
        public Announcement()
        {
            Ideas = new HashSet<IdeaInfo>();
        }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public virtual ICollection<IdeaInfo> Ideas { get; set; }
    }
}
