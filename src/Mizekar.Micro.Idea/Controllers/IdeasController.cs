using System;
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
        /// Get Ideas ny Ids
        /// </summary>
        /// <returns></returns>
        [HttpGet("ids")]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<List<IdeaViewPoco>>> GetIdeasByIds(Guid[] ids)
        {
            var query = await _ideas
                .Include(i => i.IdeaStatus)
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

            ManageRelations(id, advancedFieldPoco);

            ideaInfoEntity.Introduction = advancedFieldPoco.Introduction;
            ideaInfoEntity.Achievement = advancedFieldPoco.Achievement;
            ideaInfoEntity.Necessity = advancedFieldPoco.Necessity;
            ideaInfoEntity.Details = advancedFieldPoco.Details;
            ideaInfoEntity.Problem = advancedFieldPoco.Problem;

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void ManageRelations(Guid id, IdeaAdvancedFieldPoco advancedFieldPoco)
        {
            var scopes = _context.ScopeLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var scopeLink in scopes)
            {
                if (!advancedFieldPoco.ScopeLinks.Contains(scopeLink.ScopeId))
                {
                    scopeLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var scopeId in advancedFieldPoco.ScopeLinks)
            {
                if (scopes.FirstOrDefault(f => f.ScopeId == scopeId) == null)
                {
                    var newScopeRelation = new ScopeLink() { IdeaId = id, ScopeId = scopeId };
                    _context.Add(newScopeRelation);
                }
            }

            var departments = _context.DepartmentLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var departmentLink in departments)
            {
                if (!advancedFieldPoco.DepartmentLinks.Contains(departmentLink.DepartmentId))
                {
                    departmentLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var departmentId in advancedFieldPoco.DepartmentLinks)
            {
                if (departments.FirstOrDefault(f => f.DepartmentId == departmentId) == null)
                {
                    var newDepartmentRelation = new DepartmentLink() { IdeaId = id, DepartmentId = departmentId };
                    _context.Add(newDepartmentRelation);
                }
            }

            var subjects = _context.SubjectLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var subjectLink in subjects)
            {
                if (!advancedFieldPoco.SubjectLinks.Contains(subjectLink.SubjectId))
                {
                    subjectLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var subjectId in advancedFieldPoco.SubjectLinks)
            {
                if (subjects.FirstOrDefault(f => f.SubjectId == subjectId) == null)
                {
                    var newSubjectRelation = new SubjectLink() { IdeaId = id, SubjectId = subjectId };
                    _context.Add(newSubjectRelation);
                }
            }

            var strategies = _context.StrategyLinks.Where(q => q.IdeaId == id).ToList();
            // remove none selected
            foreach (var strategyLink in strategies)
            {
                if (!advancedFieldPoco.StrategyLinks.Contains(strategyLink.StrategyId))
                {
                    strategyLink.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var strategyId in advancedFieldPoco.StrategyLinks)
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