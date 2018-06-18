using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Requirements
{
    public class RequirementViewPoco
    {
        public Guid Id { get; set; }
        public RequirementPoco Requirement { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
