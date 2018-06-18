using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Similar
{
    public class SimilarIdeaViewPoco
    {
        public Guid Id { get; set; }
        public SimilarIdeaPoco SimilarIdea { get; set; }

        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
