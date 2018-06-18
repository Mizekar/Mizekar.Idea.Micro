using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities
{
    /// <summary>
    /// وضعیت ایده
    /// </summary>
    public class IdeaStatus : BusinessBaseEntity
    {
        public IdeaStatus()
        {
            Ideas = new HashSet<IdeaInfo>();
        }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string HexColor { get; set; }

        public virtual ICollection<IdeaInfo> Ideas { get; set; }
    }
}
