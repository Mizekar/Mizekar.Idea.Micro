using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.IdeaOptions
{
    public class IdeaOptionSetItemViewPoco
    {
        public Guid Id { get; set; }
        public IdeaOptionSetItemPoco IdeaOptionSetItem { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
