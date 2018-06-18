﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Data;
using Mizekar.Core.Model.Api;
using Mizekar.Core.Model.Api.Response;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Models;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public IdeasController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return new IdeaViewPoco()
            {
                Id = ideaInfo.Id,
                Idea = _mapper.Map<IdeaPoco>(ideaInfo),
                AdvancedField = _mapper.Map<IdeaAdvancedFieldPoco>(ideaInfo),
                IdeaStatus = _mapper.Map<IdeaStatusPoco>(ideaInfo.IdeaStatus),
                SocialStatistic = _mapper.Map<IdeaSocialStatisticPoco>(ideaInfo.SocialStatistics.First()),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaInfo)
            };
        }

        /// <summary>
        /// Get ideas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaViewPoco>>> Getideas(int pageNumber, int pageSize)
        {
            var query = _ideas.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Idea By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaViewPoco), 200)]
        public async Task<ActionResult<IdeaViewPoco>> GetIdeaInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfo = await _ideas.Include(i => i.IdeaStatus).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaInfo == null)
            {
                return NotFound();
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
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> PutIdeaInfo([FromRoute] Guid id, [FromBody] IdeaPoco ideaPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaInfoEntity = await _ideas.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaInfoEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(ideaPoco, ideaInfoEntity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Update Idea Advanced Fields
        /// </summary>
        /// <param name="id"></param>
        /// <param name="advancedFieldPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}/advanced")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> PutIdeaAdvancedFields([FromRoute] Guid id, [FromBody] IdeaAdvancedFieldPoco advancedFieldPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaInfoEntity = await _ideas.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaInfoEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(advancedFieldPoco, ideaInfoEntity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create Idea
        /// </summary>
        /// <param name="ideaPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> PostIdea([FromBody] IdeaPoco ideaPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfoEntity = _mapper.Map<IdeaInfo>(ideaPoco);
            _ideas.Add(ideaInfoEntity);
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
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteIdea([FromRoute] Guid id)
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
                return NotFound();
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

            return Ok(ideaInfo);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}