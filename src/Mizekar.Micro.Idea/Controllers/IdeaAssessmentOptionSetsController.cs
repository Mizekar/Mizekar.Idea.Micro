using System;
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
using Mizekar.Micro.Idea.Models.IdeaAssessmentOptions;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// Idea Assessment OptionSets Management - مدیریت مواردهای ارزیابی
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "IdeaAssessmentOptionSets", Name = "IdeaAssessmentOptionSets", Description = "Idea Assessment OptionSets Management - مدیریت مواردهای ارزیابی")]
    public class IdeaAssessmentOptionSetsController : ControllerBase
    {
        private readonly DbSet<IdeaAssessmentOptionSet> _ideaAssessmentOptionSets;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public IdeaAssessmentOptionSetsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _ideaAssessmentOptionSets = _context.IdeaAssessmentOptionSets;
        }

        private async Task<Paged<IdeaAssessmentOptionSetViewPoco>> ToPaged(IQueryable<IdeaAssessmentOptionSet> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<IdeaAssessmentOptionSetViewPoco>();
            foreach (var ideaAssessmentOptionSet in entities)
            {
                models.Add(ConvertToModel(ideaAssessmentOptionSet));
            }

            var resultPaged = new Paged<IdeaAssessmentOptionSetViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private IdeaAssessmentOptionSetViewPoco ConvertToModel(IdeaAssessmentOptionSet ideaAssessmentOptionSet)
        {
            return new IdeaAssessmentOptionSetViewPoco()
            {
                Id = ideaAssessmentOptionSet.Id,
                IdeaAssessmentOptionSet = _mapper.Map<IdeaAssessmentOptionSetPoco>(ideaAssessmentOptionSet),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaAssessmentOptionSet)
            };
        }

        /// <summary>
        /// Get ideaAssessmentOptionSets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaAssessmentOptionSetViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaAssessmentOptionSetViewPoco>>> GetIdeaAssessmentOptionSets(int pageNumber, int pageSize)
        {
            var query = _ideaAssessmentOptionSets.AsNoTracking()
                .OrderBy(o=>o.Order)
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get IdeaAssessmentOptionSet By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaAssessmentOptionSetViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<IdeaAssessmentOptionSetViewPoco>> GetIdeaAssessmentOptionSet([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAssessmentOptionSet = await _ideaAssessmentOptionSets.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaAssessmentOptionSet == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaAssessmentOptionSet);
            return Ok(poco);
        }

        /// <summary>
        /// Update IdeaAssessmentOptionSet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ideaAssessmentOptionSetPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaAssessmentOptionSet([FromRoute] Guid id, [FromBody] IdeaAssessmentOptionSetPoco ideaAssessmentOptionSetPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaAssessmentOptionSetEntity = await _ideaAssessmentOptionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaAssessmentOptionSetEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaAssessmentOptionSetPoco, ideaAssessmentOptionSetEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create IdeaAssessmentOptionSet
        /// </summary>
        /// <param name="ideaAssessmentOptionSetPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostIdeaAssessmentOptionSet([FromBody] IdeaAssessmentOptionSetPoco ideaAssessmentOptionSetPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAssessmentOptionSetEntity = _mapper.Map<IdeaAssessmentOptionSet>(ideaAssessmentOptionSetPoco);
            _ideaAssessmentOptionSets.Add(ideaAssessmentOptionSetEntity);
            await _context.SaveChangesAsync();

            return Ok(ideaAssessmentOptionSetEntity.Id);
        }

        /// <summary>
        /// Delete IdeaAssessmentOptionSet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteIdeaAssessmentOptionSet([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAssessmentOptionSet = await _ideaAssessmentOptionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaAssessmentOptionSet == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaAssessmentOptionSet);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}