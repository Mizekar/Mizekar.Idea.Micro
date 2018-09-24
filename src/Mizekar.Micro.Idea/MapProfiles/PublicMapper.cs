using System.Linq;
using AutoMapper;
using Mizekar.Core.Model.Api;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Data.Entities.Functional;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Announcements;
using Mizekar.Micro.Idea.Models.IdeaAssessmentOptions;
using Mizekar.Micro.Idea.Models.IdeaOptions;
using Mizekar.Micro.Idea.Models.Operational;
using Mizekar.Micro.Idea.Models.Participations;
using Mizekar.Micro.Idea.Models.Permissions;
using Mizekar.Micro.Idea.Models.Profiles;
using Mizekar.Micro.Idea.Models.Requirements;
using Mizekar.Micro.Idea.Models.Services;
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
                .ForMember(desc => desc.StrategyLinks, src => src.MapFrom(m => m.StrategyLinks.Where(q => !q.IsDeleted).Select(s => s.StrategyId)))
                .ForMember(desc => desc.OptionItemIds, src => src.MapFrom(m => m.IdeaOptionSelections.Where(q => !q.IsDeleted).Select(s => s.IdeaOptionSetItemId)));
            CreateMap<IdeaInfo, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<IdeaSocialStatistic, IdeaSocialStatisticPoco>(MemberList.Destination);

            CreateMap<IdeaStatusPoco, IdeaStatus>(MemberList.Source);
            CreateMap<IdeaStatus, IdeaStatusPoco>(MemberList.Destination);
            CreateMap<IdeaStatus, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<AnnouncementPoco, Announcement>(MemberList.Source);
            CreateMap<Announcement, AnnouncementPoco>(MemberList.Destination);
            CreateMap<Announcement, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<ServicePoco, Service>(MemberList.Source);
            CreateMap<Service, ServicePoco>(MemberList.Destination);
            CreateMap<Service, BusinessBaseInfo>(MemberList.Destination);

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

            CreateMap<IdeaOptionSetPoco, IdeaOptionSet>(MemberList.Source);
            CreateMap<IdeaOptionSet, IdeaOptionSetPoco>(MemberList.Destination);
            CreateMap<IdeaOptionSet, BusinessBaseInfo>(MemberList.Destination);
            CreateMap<IdeaOptionSetItemPoco, IdeaOptionSetItem>(MemberList.Source);
            CreateMap<IdeaOptionSetItem, IdeaOptionSetItemPoco>(MemberList.Destination);
            CreateMap<IdeaOptionSetItem, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<IdeaAssessmentOptionSetPoco, IdeaAssessmentOptionSet>(MemberList.Source);
            CreateMap<IdeaAssessmentOptionSet, IdeaAssessmentOptionSetPoco>(MemberList.Destination);
            CreateMap<IdeaAssessmentOptionSet, BusinessBaseInfo>(MemberList.Destination);
            CreateMap<IdeaAssessmentOptionSetItemPoco, IdeaAssessmentOptionSetItem>(MemberList.Source);
            CreateMap<IdeaAssessmentOptionSetItem, IdeaAssessmentOptionSetItemPoco>(MemberList.Destination);
            CreateMap<IdeaAssessmentOptionSetItem, BusinessBaseInfo>(MemberList.Destination);
            CreateMap<IdeaAssessmentScorePoco, IdeaAssessmentScore>(MemberList.Source);
            CreateMap<IdeaAssessmentScore, IdeaAssessmentScorePoco>(MemberList.Destination);
            CreateMap<IdeaAssessmentScore, IdeaAssessmentScoreSimplePoco>(MemberList.Destination);
            CreateMap<IdeaAssessmentScore, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<ProfilePoco, Data.Entities.Functional.Profile>(MemberList.Source);
            CreateMap<Data.Entities.Functional.Profile, ProfilePoco>(MemberList.Destination);
            CreateMap<Data.Entities.Functional.Profile, BusinessBaseInfo>(MemberList.Destination);

            CreateMap<PermissionPoco, Permission>(MemberList.Source);
            CreateMap<Permission, PermissionPoco>(MemberList.Destination);
            CreateMap<Permission, BusinessBaseInfo>(MemberList.Destination);
        }
    }
}
