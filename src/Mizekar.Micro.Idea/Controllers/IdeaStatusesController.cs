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
using Mizekar.Micro.Idea.Models;
using Mizekar.Micro.Idea.Models.Operational;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// ideaStatuses Management - مدیریت وضعیت
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "IdeaStatuses", Name = "IdeaStatuses", Description = "IdeaStatuses Management - مدیریت وضعیت")]
    public class IdeaStatusesController : ControllerBase
    {
        private readonly DbSet<IdeaStatus> _ideaStatuses;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public IdeaStatusesController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _ideaStatuses = _context.IdeaStatuses;
        }

        private async Task<Paged<IdeaStatusViewPoco>> ToPaged(IQueryable<IdeaStatus> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<IdeaStatusViewPoco>();
            foreach (var ideaStatusInfo in entities)
            {
                models.Add(ConvertToModel(ideaStatusInfo));
            }

            var resultPaged = new Paged<IdeaStatusViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private IdeaStatusViewPoco ConvertToModel(IdeaStatus ideaStatusInfo)
        {
            return new IdeaStatusViewPoco()
            {
                Id = ideaStatusInfo.Id,
                IdeaStatus = _mapper.Map<IdeaStatusPoco>(ideaStatusInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaStatusInfo)
            };
        }

        /// <summary>
        /// Get ideaStatuses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaStatusViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaStatusViewPoco>>> GetIdeaStatuses(int pageNumber, int pageSize)
        {
            var query = _ideaStatuses.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get IdeaStatus By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaStatusViewPoco), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<IdeaStatusViewPoco>> GetIdeaStatusInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaStatusInfo = await _ideaStatuses.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaStatusInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaStatusInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update IdeaStatus
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ideaStatusPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaStatusInfo([FromRoute] Guid id, [FromBody] IdeaStatusPoco ideaStatusPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaStatusInfoEntity = await _ideaStatuses.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaStatusInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaStatusPoco, ideaStatusInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create IdeaStatus
        /// </summary>
        /// <param name="ideaStatusPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostIdeaStatus([FromBody] IdeaStatusPoco ideaStatusPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaStatusInfoEntity = _mapper.Map<IdeaStatus>(ideaStatusPoco);
            _ideaStatuses.Add(ideaStatusInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(ideaStatusInfoEntity.Id);
        }

        /// <summary>
        /// Delete IdeaStatus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteIdeaStatus([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaStatusInfo = await _ideaStatuses.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaStatusInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaStatusInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}