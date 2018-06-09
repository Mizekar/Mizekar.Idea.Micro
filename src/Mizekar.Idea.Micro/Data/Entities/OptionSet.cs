using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Core.Data;

namespace Mizekar.Idea.Micro.Data.Entities
{
    public class OptionSet : BusinessBaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public bool IsMultiSelect { get; set; }
    }

    public class OptionSetItem : BusinessBaseEntity
    {
        public Guid OptionSetId { get; set; }
        public OptionSet OptionSet { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string HexColor { get; set; }
    }
}
