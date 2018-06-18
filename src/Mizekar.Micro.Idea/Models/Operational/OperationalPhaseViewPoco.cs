using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Operational
{
    public class OperationalPhaseViewPoco
    {
        public Guid Id { get; set; }
        public OperationalPhasePoco OperationalPhase { get; set; }
        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}
