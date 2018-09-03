using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Permissions
{
    public class PermissionViewPoco
    {
        public Guid Id { get; set; }
        public PermissionPoco Permission { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
