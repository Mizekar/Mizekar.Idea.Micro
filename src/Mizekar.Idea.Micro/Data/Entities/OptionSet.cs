using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class OptionSet : BusinessBaseEntity
    {
        public OptionSet()
        {
            IdeaInfoOptionSetRelations = new HashSet<IdeaInfoOptionSetRelation>();
        }


        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
        public string Category { get; set; }
        public bool IsMultiSelect { get; set; }

        public virtual ICollection<IdeaInfoOptionSetRelation> IdeaInfoOptionSetRelations { get; set; }
    }
}
