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
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// operationalPhases Management - مدیریت فازهای اجرایی
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "OperationalPhases", Name = "OperationalPhases", Description = "OperationalPhases Management - مدیریت ایده ها")]
    public class OperationalPhasesController : ControllerBase
    {
        private readonly DbSet<OperationalPhase> _operationalPhases;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public OperationalPhasesController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _operationalPhases = _context.OperationalPhases;
        }

        private async Task<Paged<OperationalPhaseViewPoco>> ToPaged(IQueryable<OperationalPhase> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<OperationalPhaseViewPoco>();
            foreach (var operationalPhaseInfo in entities)
            {
                models.Add(ConvertToModel(operationalPhaseInfo));
            }

            var resultPaged = new Paged<OperationalPhaseViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private OperationalPhaseViewPoco ConvertToModel(OperationalPhase operationalPhaseInfo)
        {
            return new OperationalPhaseViewPoco()
            {
                Id = operationalPhaseInfo.Id,
                OperationalPhase = _mapper.Map<OperationalPhasePoco>(operationalPhaseInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(operationalPhaseInfo)
            };
        }

        /// <summary>
        /// Get operationalPhases
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<OperationalPhaseViewPoco>), 200)]
        public async Task<ActionResult<Paged<OperationalPhaseViewPoco>>> GetOperationalPhases(int pageNumber, int pageSize)
        {
            var query = _operationalPhases.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get operationalPhases
        /// </summary>
        /// <returns></returns>
        [HttpGet("ideaId/{ideaId}")]
        [ProducesResponseType(typeof(Paged<OperationalPhaseViewPoco>), 200)]
        public async Task<ActionResult<Paged<OperationalPhaseViewPoco>>> GetOperationalPhasesByIdeaId([FromRoute] Guid ideaId, int pageNumber, int pageSize)
        {
            var query = _operationalPhases.AsNoTracking().Where(q => q.IdeaId == ideaId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get OperationalPhase By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OperationalPhaseViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<OperationalPhaseViewPoco>> GetOperationalPhaseInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operationalPhaseInfo = await _operationalPhases.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (operationalPhaseInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(operationalPhaseInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update OperationalPhase
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operationalPhasePoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutOperationalPhaseInfo([FromRoute] Guid id, [FromBody] OperationalPhasePoco operationalPhasePoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operationalPhaseInfoEntity = await _operationalPhases.FirstOrDefaultAsync(q => q.Id == id);
            if (operationalPhaseInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(operationalPhasePoco, operationalPhaseInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create OperationalPhase
        /// </summary>
        /// <param name="operationalPhasePoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostOperationalPhase([FromBody] OperationalPhasePoco operationalPhasePoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operationalPhaseInfoEntity = _mapper.Map<OperationalPhase>(operationalPhasePoco);
            _operationalPhases.Add(operationalPhaseInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(operationalPhaseInfoEntity.Id);
        }

        /// <summary>
        /// Delete OperationalPhase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteOperationalPhase([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operationalPhaseInfo = await _operationalPhases.FirstOrDefaultAsync(q => q.Id == id);
            if (operationalPhaseInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(operationalPhaseInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}