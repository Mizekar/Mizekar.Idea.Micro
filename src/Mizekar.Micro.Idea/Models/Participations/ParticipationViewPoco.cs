using System;
using Mizekar.Core.Model.Api;

namespace Mizekar.Micro.Idea.Models.Participations
{
    public class ParticipationViewPoco
    {
        public Guid Id { get; set; }
        public ParticipationPoco Participation { get; set; }

        public BusinessBaseInfo BusinessBaseInfo { get; set; }
    }
}