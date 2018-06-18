using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Similar
{
    public class SimilarIdeaViewPoco
    {
        public Guid Id { get; set; }
        public SimilarIdeaPoco SimilarInfo { get; set; }

        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
