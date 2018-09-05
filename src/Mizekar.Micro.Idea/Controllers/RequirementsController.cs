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
using Mizekar.Micro.Idea.Models.Operational;
using Mizekar.Micro.Idea.Models.Requirements;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// requirements Management - مدیریت نیازمندی
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Requirements", Name = "Requirements", Description = "Requirements Management - مدیریت نیازمندی")]
    public class RequirementsController : ControllerBase
    {
        private readonly DbSet<Requirement> _requirements;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public RequirementsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _requirements = _context.Requirements;
        }

        private async Task<Paged<RequirementViewPoco>> ToPaged(IQueryable<Requirement> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<RequirementViewPoco>();
            foreach (var requirementInfo in entities)
            {
                models.Add(ConvertToModel(requirementInfo));
            }

            var resultPaged = new Paged<RequirementViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private RequirementViewPoco ConvertToModel(Requirement requirementInfo)
        {
            return new RequirementViewPoco()
            {
                Id = requirementInfo.Id,
                Requirement = _mapper.Map<RequirementPoco>(requirementInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(requirementInfo)
            };
        }

        /// <summary>
        /// Get requirements
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<RequirementViewPoco>), 200)]
        public async Task<ActionResult<Paged<RequirementViewPoco>>> GetRequirements(int pageNumber, int pageSize)
        {
            var query = _requirements.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get requirements
        /// </summary>
        /// <returns></returns>
        [HttpGet("ideaId/{ideaId}")]
        [ProducesResponseType(typeof(Paged<RequirementViewPoco>), 200)]
        public async Task<ActionResult<Paged<RequirementViewPoco>>> GetRequirementsByIdeaId([FromRoute] Guid ideaId, int pageNumber, int pageSize)
        {
            var query = _requirements.AsNoTracking().Where(q => q.IdeaId == ideaId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Requirement By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RequirementViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<RequirementViewPoco>> GetRequirementInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requirementInfo = await _requirements.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (requirementInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(requirementInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Requirement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requirementPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutRequirementInfo([FromRoute] Guid id, [FromBody] RequirementPoco requirementPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var requirementInfoEntity = await _requirements.FirstOrDefaultAsync(q => q.Id == id);
            if (requirementInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(requirementPoco, requirementInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Requirement
        /// </summary>
        /// <param name="requirementPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostRequirement([FromBody] RequirementPoco requirementPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requirementInfoEntity = _mapper.Map<Requirement>(requirementPoco);
            _requirements.Add(requirementInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(requirementInfoEntity.Id);
        }

        /// <summary>
        /// Delete Requirement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteRequirement([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requirementInfo = await _requirements.FirstOrDefaultAsync(q => q.Id == id);
            if (requirementInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(requirementInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}