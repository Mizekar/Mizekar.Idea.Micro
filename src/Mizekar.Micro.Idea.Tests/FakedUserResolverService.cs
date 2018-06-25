using Mizekar.Core.Data.Services;

namespace Mizekar.Micro.Idea.Tests
{
    class FakedUserResolverService : IUserResolverService
    {
        public FakedUserResolverService(long userId)
        {
            UserId = userId;
        }
        public long UserId { get; set; }
    }
}
