using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mizekar.Core.Model.Api;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Models;

namespace Mizekar.Micro.Idea.Controllers
{
    public class IdeasControllerMapper : AutoMapper.Profile
    {
        public IdeasControllerMapper()
        {
            CreateMap<IdeaPoco, IdeaInfo>(MemberList.Source);
            CreateMap<IdeaAdvancedFieldPoco, IdeaInfo>(MemberList.Source);
            CreateMap<IdeaInfo, IdeaPoco>(MemberList.Destination);
            CreateMap<IdeaInfo, IdeaAdvancedFieldPoco>(MemberList.Destination);
            CreateMap<IdeaInfo, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<IdeaSocialStatistic, IdeaSocialStatisticPoco>(MemberList.Destination);

            CreateMap<IdeaStatusPoco, IdeaStatus>(MemberList.Source);
            CreateMap<IdeaStatus, IdeaStatusPoco>(MemberList.Destination);
            CreateMap<IdeaStatus, BusinessBaseInfo>(MemberList.Destination);
        }
    }
}
