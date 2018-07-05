using System.Linq;
using AutoMapper;
using Mizekar.Core.Model.Api;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Operational;
using Mizekar.Micro.Idea.Models.Participations;
using Mizekar.Micro.Idea.Models.Requirements;
using Mizekar.Micro.Idea.Models.Similar;

namespace Mizekar.Micro.Idea.MapProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicMapper : AutoMapper.Profile
    {
        public PublicMapper()
        {
            CreateMap<IdeaPoco, IdeaInfo>(MemberList.Source);
            //CreateMap<IdeaAdvancedFieldPoco, IdeaInfo>(MemberList.Source)
            //    .ForMember(desc => desc.DepartmentLinks, src => src.MapFrom(m => m.DepartmentLinks.Select(s => s.DepartmentId)))
            //    .ForMember(desc => desc.ScopeLinks, src => src.MapFrom(m => m.ScopeLinks.Select(s => s.ScopeId)))
            //    .ForMember(desc => desc.SubjectLinks, src => src.MapFrom(m => m.SubjectLinks.Select(s => s.SubjectId)))
            //    .ForMember(desc => desc.StrategyLinks, src => src.MapFrom(m => m.StrategyLinks.Select(s => s.StrategyId)));
            CreateMap<IdeaInfo, IdeaPoco>(MemberList.Destination);
            CreateMap<IdeaInfo, IdeaAdvancedFieldPoco>(MemberList.Destination)
                .ForMember(desc => desc.DepartmentLinks, src => src.MapFrom(m => m.DepartmentLinks.Where(q => !q.IsDeleted).Select(s => s.DepartmentId)))
                .ForMember(desc => desc.ScopeLinks, src => src.MapFrom(m => m.ScopeLinks.Where(q => !q.IsDeleted).Select(s => s.ScopeId)))
                .ForMember(desc => desc.SubjectLinks, src => src.MapFrom(m => m.SubjectLinks.Where(q => !q.IsDeleted).Select(s => s.SubjectId)))
                .ForMember(desc => desc.StrategyLinks, src => src.MapFrom(m => m.StrategyLinks.Where(q => !q.IsDeleted).Select(s => s.StrategyId)));
            CreateMap<IdeaInfo, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<IdeaSocialStatistic, IdeaSocialStatisticPoco>(MemberList.Destination);

            CreateMap<IdeaStatusPoco, IdeaStatus>(MemberList.Source);
            CreateMap<IdeaStatus, IdeaStatusPoco>(MemberList.Destination);
            CreateMap<IdeaStatus, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<OperationalPhasePoco, OperationalPhase>(MemberList.Source);
            CreateMap<OperationalPhase, OperationalPhasePoco>(MemberList.Destination);
            CreateMap<OperationalPhase, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<RequirementPoco, Requirement>(MemberList.Source);
            CreateMap<Requirement, RequirementPoco>(MemberList.Destination);
            CreateMap<Requirement, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<SimilarIdeaPoco, SimilarIdea>(MemberList.Source);
            CreateMap<SimilarIdea, SimilarIdeaPoco>(MemberList.Destination);
            CreateMap<SimilarIdea, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<ParticipationPoco, Participation>(MemberList.Source);
            CreateMap<Participation, ParticipationPoco>(MemberList.Destination);
            CreateMap<Participation, BusinessBaseInfo>(MemberList.Destination);
        }
    }
}
