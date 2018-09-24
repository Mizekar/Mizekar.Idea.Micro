﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Data;
using Mizekar.Core.Data.Services;
using Mizekar.Core.Model.Api;
using Mizekar.Core.Model.Api.Response;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Announcements;
using Mizekar.Micro.Idea.Models.IdeaAssessmentOptions;
using Mizekar.Micro.Idea.Models.Services;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// ideas Management - مدیریت ایده ها
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Ideas", Name = "Ideas", Description = "Ideas Management - مدیریت ایده ها")]
    public class IdeasController : ControllerBase
    {
        private readonly DbSet<IdeaInfo> _ideas;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="userResolverService"></param>
        public IdeasController(IdeaDbContext context, IMapper mapper, IUserResolverService userResolverService)
        {
            _context = context;
            _mapper = mapper;
            _userResolverService = userResolverService;
            _ideas = _context.IdeaInfos;
        }

        private async Task<Paged<IdeaViewPoco>> ToPaged(IQueryable<IdeaInfo> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<IdeaViewPoco>();
            foreach (var ideaInfo in entities)
            {
                models.Add(ConvertToModel(ideaInfo));
            }

            var resultPaged = new Paged<IdeaViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private IdeaViewPoco ConvertToModel(IdeaInfo ideaInfo)
        {
            var poco = new IdeaViewPoco();
            poco.Id = ideaInfo.Id;
            poco.Idea = _mapper.Map<IdeaPoco>(ideaInfo);
            poco.AdvancedField = _mapper.Map<IdeaAdvancedFieldPoco>(ideaInfo);
            poco.BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaInfo);

            var scores = ideaInfo.IdeaAssessmentScores.ToList();
            poco.IdeaAssessmentScores = _mapper.Map<List<IdeaAssessmentScoreSimplePoco>>(scores);

            var status = ideaInfo.IdeaStatus;
            poco.IdeaStatus = _mapper.Map<IdeaStatusPoco>(status);

            var announcement = ideaInfo.Announcement;
            poco.Announcement = announcement == null ? null : _mapper.Map<AnnouncementPoco>(announcement);

            var service = ideaInfo.Service;
            poco.Service = service == null ? null : _mapper.Map<ServicePoco>(service);

            var statistic = ideaInfo.SocialStatistics.FirstOrDefault();
            poco.SocialStatistic = statistic != null ? _mapper.Map<IdeaSocialStatisticPoco>(statistic) : new IdeaSocialStatisticPoco();

            return poco;
        }

        /// <summary>
        /// Get Last Ideas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaViewPoco>>> GetLastIdeas(int pageNumber, int pageSize)
        {
            var query = _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .OrderByDescending(o => o.CreatedOn);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get My Last Ideas
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaViewPoco>>> GetMyLastIdeas(int pageNumber, int pageSize)
        {
            var query = _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .OrderByDescending(o => o.CreatedOn)
                .Where(q => q.OwnerId == _userResolverService.UserId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get My Last Ideas
        /// </summary>
        /// <returns></returns>
        [HttpGet("userid/{userId}/")]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaViewPoco>>> GetLastIdeasByUser([FromRoute] long userId, int pageNumber, int pageSize)
        {
            var query = _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .OrderByDescending(o => o.CreatedOn)
                .Where(q => q.OwnerId == userId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Last Ideas By Service
        /// </summary>
        /// <returns></returns>
        [HttpGet("serviceId/{serviceId}/")]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaViewPoco>>> GetLastIdeasByService([FromRoute] Guid serviceId, int pageNumber, int pageSize)
        {
            var query = _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .OrderByDescending(o => o.CreatedOn)
                .Where(q => q.ServiceId == serviceId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Last Ideas By Announcement
        /// </summary>
        /// <returns></returns>
        [HttpGet("announcementId/{announcementId}/")]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaViewPoco>>> GetLastIdeasByAnnouncement([FromRoute] Guid announcementId, int pageNumber, int pageSize)
        {
            var query = _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .OrderByDescending(o => o.CreatedOn)
                .Where(q => q.AnnouncementId == announcementId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Ideas ny Ids
        /// </summary>
        /// <returns></returns>
        [HttpGet("ids")]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<List<IdeaViewPoco>>> GetIdeasByIds(Guid[] ids)
        {
            var query = await _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .OrderByDescending(o => o.CreatedOn)
                .Where(q => ids.Contains(q.Id)).ToListAsync();

            var models = new List<IdeaViewPoco>();
            foreach (var ideaInfo in query)
            {
                models.Add(ConvertToModel(ideaInfo));
            }
            return Ok(models);
        }

        /// <summary>
        /// Get Idea By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaViewPoco), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<IdeaViewPoco>> GetIdeaInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfo = await _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                //.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ideaInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Idea
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ideaPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaInfo([FromRoute] Guid id, [FromBody] IdeaPoco ideaPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfoEntity = await _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (ideaInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaPoco, ideaInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Update Statistic Comments
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        [HttpPut("statistic/comment/{ideaId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> UpdateStatisticComments([FromRoute] Guid ideaId, bool increment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var socialStatistic = await _context.IdeaSocialStatistics.FirstOrDefaultAsync(q => q.IdeaId == ideaId);
            if (socialStatistic == null)
            {
                return NotFound(ideaId);
            }
            socialStatistic.CommentCount = increment ? socialStatistic.CommentCount + 1 : socialStatistic.CommentCount - 1;
            await _context.SaveChangesAsync();

            return Ok(ideaId);
        }

        /// <summary>
        /// Update Statistic Likes
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        [HttpPut("statistic/like/{ideaId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> UpdateStatisticLikes([FromRoute] Guid ideaId, bool increment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var socialStatistic = await _context.IdeaSocialStatistics.FirstOrDefaultAsync(q => q.IdeaId == ideaId);
            if (socialStatistic == null)
            {
                return NotFound(ideaId);
            }
            socialStatistic.LikeCount = increment ? socialStatistic.LikeCount + 1 : socialStatistic.LikeCount - 1;
            await _context.SaveChangesAsync();

            return Ok(ideaId);
        }

        /// <summary>
        /// Update Statistic Views
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        [HttpPut("statistic/view/{ideaId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> UpdateStatisticViews([FromRoute] Guid ideaId, bool increment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var socialStatistic = await _context.IdeaSocialStatistics.FirstOrDefaultAsync(q => q.IdeaId == ideaId);
            if (socialStatistic == null)
            {
                return NotFound(ideaId);
            }
            socialStatistic.ViewCount = increment ? socialStatistic.ViewCount + 1 : socialStatistic.ViewCount - 1;
            await _context.SaveChangesAsync();

            return Ok(ideaId);
        }

        /// <summary>
        /// Update Statistic Scores
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="newScore"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        [HttpPut("statistic/score/{ideaId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> UpdateStatisticScores([FromRoute] Guid ideaId, int newScore, bool increment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var socialStatistic = await _context.IdeaSocialStatistics.FirstOrDefaultAsync(q => q.IdeaId == ideaId);
            if (socialStatistic == null)
            {
                return NotFound(ideaId);
            }
            socialStatistic.ScoreSum = increment ? socialStatistic.ScoreSum + newScore : socialStatistic.ScoreSum - newScore;
            await _context.SaveChangesAsync();

            return Ok(ideaId);
        }

        /// <summary>
        /// Update Idea Advanced Fields
        /// </summary>
        /// <param name="id"></param>
        /// <param name="advancedFieldPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}/advanced")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaAdvancedFields([FromRoute] Guid id, [FromBody] IdeaAdvancedFieldPoco advancedFieldPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaInfoEntity = await _ideas
                .Include(i => i.IdeaStatus)
                .Include(i => i.Service)
                .Include(i => i.Announcement)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.ScopeLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.DepartmentLinks)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (ideaInfoEntity == null)
            {
                return NotFound(id);
            }

            //_mapper.Map(advancedFieldPoco, ideaInfoEntity);
            ManageRelationsStrategys(id, advancedFieldPoco.StrategyLinks);
            ManageRelationsSubjects(id, advancedFieldPoco.SubjectLinks);
            ManageRelationsDepartments(id, advancedFieldPoco.DepartmentLinks);
            ManageRelationsScopes(id, advancedFieldPoco.ScopeLinks);
            ManageOptions(id, advancedFieldPoco.OptionItemIds);

            ideaInfoEntity.Introduction = advancedFieldPoco.Introduction;
            ideaInfoEntity.Achievement = advancedFieldPoco.Achievement;
            ideaInfoEntity.Necessity = advancedFieldPoco.Necessity;
            ideaInfoEntity.Details = advancedFieldPoco.Details;
            ideaInfoEntity.Problem = advancedFieldPoco.Problem;

            ideaInfoEntity.CountryId = advancedFieldPoco.CountryId;
            ideaInfoEntity.StateId = advancedFieldPoco.StateId;
            ideaInfoEntity.CityId = advancedFieldPoco.CityId;
            ideaInfoEntity.VillageId = advancedFieldPoco.VillageId;

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void ManageOptions(Guid id, List<Guid> newOptionItemIds)
        {
            if (newOptionItemIds == null) return;

            var currentoptionSelections = _context.IdeaOptionSelections.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var optionSelection in currentoptionSelections)
            {
                if (!newOptionItemIds.Contains(optionSelection.IdeaOptionSetItemId))
                {
                    optionSelection.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var optionItemIdId in newOptionItemIds)
            {
                if (currentoptionSelections.FirstOrDefault(f => f.IdeaOptionSetItemId == optionItemIdId) == null)
                {
                    var optionSetItem = _context.IdeaOptionSetItems.FirstOrDefault(q => q.Id == optionItemIdId);
                    var newOptionSelection = new IdeaOptionSelection()
                    {
                        IdeaId = id,
                        IdeaOptionSetId = optionSetItem.IdeaOptionSetId,
                        IdeaOptionSetItemId = optionItemIdId
                    };
                    _context.Add(newOptionSelection);
                }
            }
        }

        private void ManageRelationsScopes(Guid id, List<Guid> scopeLinks)
        {
            if (scopeLinks == null) return;

            var scopes = _context.ScopeLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var scopeLink in scopes)
            {
                if (!scopeLinks.Contains(scopeLink.ScopeId))
                {
                    scopeLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var scopeId in scopeLinks)
            {
                if (scopes.FirstOrDefault(f => f.ScopeId == scopeId) == null)
                {
                    var newScopeRelation = new ScopeLink() { IdeaId = id, ScopeId = scopeId };
                    _context.Add(newScopeRelation);
                }
            }
        }

        private void ManageRelationsDepartments(Guid id, List<Guid> departmentLinks)
        {
            if (departmentLinks == null) return;

            var departments = _context.DepartmentLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var departmentLink in departments)
            {
                if (!departmentLinks.Contains(departmentLink.DepartmentId))
                {
                    departmentLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var departmentId in departmentLinks)
            {
                if (departments.FirstOrDefault(f => f.DepartmentId == departmentId) == null)
                {
                    var newDepartmentRelation = new DepartmentLink() { IdeaId = id, DepartmentId = departmentId };
                    _context.Add(newDepartmentRelation);
                }
            }
        }

        private void ManageRelationsSubjects(Guid id, List<Guid> subjectLinks)
        {
            if (subjectLinks == null) return;

            var subjects = _context.SubjectLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var subjectLink in subjects)
            {
                if (!subjectLinks.Contains(subjectLink.SubjectId))
                {
                    subjectLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var subjectId in subjectLinks)
            {
                if (subjects.FirstOrDefault(f => f.SubjectId == subjectId) == null)
                {
                    var newSubjectRelation = new SubjectLink() { IdeaId = id, SubjectId = subjectId };
                    _context.Add(newSubjectRelation);
                }
            }
        }

        private void ManageRelationsStrategys(Guid id, List<Guid> strategyLinks)
        {
            if (strategyLinks == null) return;

            var strategies = _context.StrategyLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var strategyLink in strategies)
            {
                if (!strategyLinks.Contains(strategyLink.StrategyId))
                {
                    strategyLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var strategyId in strategyLinks)
            {
                if (strategies.FirstOrDefault(f => f.StrategyId == strategyId) == null)
                {
                    var newStrategyRelation = new StrategyLink() { IdeaId = id, StrategyId = strategyId };
                    _context.Add(newStrategyRelation);
                }
            }
        }

        /// <summary>
        /// Create Idea
        /// </summary>
        /// <param name="ideaPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostIdea([FromBody] IdeaPoco ideaPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfoEntity = _mapper.Map<IdeaInfo>(ideaPoco);
            _context.IdeaInfos.Add(ideaInfoEntity);
            var ideaSocialStatistic = new IdeaSocialStatistic() { Idea = ideaInfoEntity };
            _context.IdeaSocialStatistics.Add(ideaSocialStatistic);

            await _context.SaveChangesAsync();
            return Ok(ideaInfoEntity.Id);
        }

        /// <summary>
        /// Delete Idea
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteIdea([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfo = await _ideas
                .Include(i => i.Requirements)
                .Include(i => i.OperationalPhases)
                .Include(i => i.SimilarIdeas)
                .Include(i => i.IdeaAssessmentScores)
                .Include(i => i.Participations)
                .Include(i => i.IdeaOptionSelections)
                .Include(i => i.SocialStatistics)
                .Include(i => i.StrategyLinks)
                .Include(i => i.DepartmentLinks)
                .Include(i => i.SubjectLinks)
                .Include(i => i.ScopeLinks)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (ideaInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaInfo);
            ideaInfo.IdeaAssessmentScores.ToList().ForEach(MarkAsDelete);
            ideaInfo.Requirements.ToList().ForEach(MarkAsDelete);
            ideaInfo.OperationalPhases.ToList().ForEach(MarkAsDelete);
            ideaInfo.SimilarIdeas.ToList().ForEach(MarkAsDelete);
            ideaInfo.Participations.ToList().ForEach(MarkAsDelete);
            ideaInfo.IdeaOptionSelections.ToList().ForEach(MarkAsDelete);
            ideaInfo.SocialStatistics.ToList().ForEach(MarkAsDelete);
            ideaInfo.StrategyLinks.ToList().ForEach(MarkAsDelete);
            ideaInfo.DepartmentLinks.ToList().ForEach(MarkAsDelete);
            ideaInfo.SubjectLinks.ToList().ForEach(MarkAsDelete);
            ideaInfo.ScopeLinks.ToList().ForEach(MarkAsDelete);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}