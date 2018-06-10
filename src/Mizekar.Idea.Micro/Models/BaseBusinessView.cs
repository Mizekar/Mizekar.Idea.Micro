using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizekar.Idea.Micro.Models
{
    public abstract class BusinessBaseModel
    {
        public long TeamId { get; set; }
        public Guid Id { get; set; }
        
        public DateTimeOffset CreatedOn { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public long? ModifiedById { get; set; }
        public Guid RowGuid { get; set; }
    }
}
