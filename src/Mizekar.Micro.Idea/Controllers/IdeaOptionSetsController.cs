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
using Mizekar.Micro.Idea.Models.IdeaOptions;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// Idea OptionSets Management - مدیریت گزینه ها
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "IdeaOptionSets", Name = "IdeaOptionSets", Description = "Idea OptionSets Management - مدیریت گزینه ها")]
    public class IdeaOptionSetsController : ControllerBase
    {
        private readonly DbSet<IdeaOptionSet> _ideaOptionSets;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public IdeaOptionSetsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _ideaOptionSets = _context.IdeaOptionSets;
        }

        private async Task<Paged<IdeaOptionSetViewPoco>> ToPaged(IQueryable<IdeaOptionSet> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<IdeaOptionSetViewPoco>();
            foreach (var ideaOptionSetInfo in entities)
            {
                models.Add(ConvertToModel(ideaOptionSetInfo));
            }

            var resultPaged = new Paged<IdeaOptionSetViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private IdeaOptionSetViewPoco ConvertToModel(IdeaOptionSet ideaOptionSetInfo)
        {
            return new IdeaOptionSetViewPoco()
            {
                Id = ideaOptionSetInfo.Id,
                IdeaOptionSet = _mapper.Map<IdeaOptionSetPoco>(ideaOptionSetInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaOptionSetInfo)
            };
        }

        /// <summary>
        /// Get ideaOptionSets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaOptionSetViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaOptionSetViewPoco>>> GetIdeaOptionSets(int pageNumber, int pageSize)
        {
            var query = _ideaOptionSets.AsNoTracking()
                .OrderBy(o=>o.Order)
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get IdeaOptionSet By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaOptionSetViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<IdeaOptionSetViewPoco>> GetIdeaOptionSetInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetInfo = await _ideaOptionSets.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaOptionSetInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaOptionSetInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update IdeaOptionSet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ideaOptionSetPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaOptionSetInfo([FromRoute] Guid id, [FromBody] IdeaOptionSetPoco ideaOptionSetPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaOptionSetInfoEntity = await _ideaOptionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaOptionSetInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaOptionSetPoco, ideaOptionSetInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create IdeaOptionSet
        /// </summary>
        /// <param name="ideaOptionSetPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostIdeaOptionSet([FromBody] IdeaOptionSetPoco ideaOptionSetPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetInfoEntity = _mapper.Map<IdeaOptionSet>(ideaOptionSetPoco);
            _ideaOptionSets.Add(ideaOptionSetInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(ideaOptionSetInfoEntity.Id);
        }

        /// <summary>
        /// Delete IdeaOptionSet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteIdeaOptionSet([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetInfo = await _ideaOptionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaOptionSetInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaOptionSetInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}