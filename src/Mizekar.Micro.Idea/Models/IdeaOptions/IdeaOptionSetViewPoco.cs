using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.IdeaOptions
{
    public class IdeaOptionSetViewPoco
    {
        public Guid Id { get; set; }
        public IdeaOptionSetPoco IdeaOptionSet { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
