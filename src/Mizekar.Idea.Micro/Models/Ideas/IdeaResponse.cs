using Mizekar.Core.Model.Api.Response;

namespace Mizekar.Idea.Micro.Models.IdeaModels
{
    public class IdeaResponse : BaseResponseModel
    {
        public IdeaView Idea { get; set; }
    }
}