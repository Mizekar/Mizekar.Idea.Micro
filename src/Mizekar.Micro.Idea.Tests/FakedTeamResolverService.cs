using Mizekar.Core.Data.Services;

namespace Mizekar.Micro.Idea.Tests
{
    class FakedTeamResolverService : ITeamResolverService
    {
        public FakedTeamResolverService(long teamId)
        {
            TeamId = teamId;
        }
        public long? TeamId { get; set; }
    }
}