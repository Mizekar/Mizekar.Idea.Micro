using System;
using Mizekar.Core.Data;

namespace Mizekar.Micro.Idea.Data.Entities.Functional
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionOwner : BusinessBaseEntity
    {
        public virtual Permission Permission { get; set; }
        public Guid PermissionId { get; set; }

        public long UserId { get; set; }
    }
}
