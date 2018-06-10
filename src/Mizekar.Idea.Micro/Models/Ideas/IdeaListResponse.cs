using System.Collections.Generic;
using Mizekar.Core.Model.Api.Response;

namespace Mizekar.Idea.Micro.Models.IdeaModels
{
    public class IdeaListResponse : BaseResponseModel
    {
        public List<IdeaView> Ideas { get; set; }
    }
}
