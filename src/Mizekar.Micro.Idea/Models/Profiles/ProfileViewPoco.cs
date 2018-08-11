using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Profiles
{
    public class ProfileViewPoco
    {
        public Guid Id { get; set; }
        public ProfilePoco Profile { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
