using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities.Functional
{
    /// <summary>
    /// دسترسی ها
    /// </summary>
    public class Permission : BusinessBaseEntity
    {
        public Permission()
        {
            PermissionOwners = new HashSet<PermissionOwner>();
        }

        public int Order { get; set; }
        /// <summary>
        /// en
        /// </summary>
        public string Name { get; set; }
        public string Title { get; set; }
        public virtual ICollection<PermissionOwner> PermissionOwners { get; set; }
    }
}
