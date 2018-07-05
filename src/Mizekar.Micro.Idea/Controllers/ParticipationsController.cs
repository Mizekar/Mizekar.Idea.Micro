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
using Mizekar.Micro.Idea.Models.Participations;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// participations Management - مدیریت مشارکت
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Participations", Name = "Participations", Description = "Participations Management - مدیریت مشارکت")]
    public class ParticipationsController : ControllerBase
    {
        private readonly DbSet<Participation> _participations;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ParticipationsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _participations = _context.Participations;
        }

        private async Task<Paged<ParticipationViewPoco>> ToPaged(IQueryable<Participation> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<ParticipationViewPoco>();
            foreach (var participationInfo in entities)
            {
                models.Add(ConvertToModel(participationInfo));
            }

            var resultPaged = new Paged<ParticipationViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private ParticipationViewPoco ConvertToModel(Participation participationInfo)
        {
            return new ParticipationViewPoco()
            {
                Id = participationInfo.Id,
                Participation = _mapper.Map<ParticipationPoco>(participationInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(participationInfo)
            };
        }

        /// <summary>
        /// Get participations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<ParticipationViewPoco>), 200)]
        public async Task<ActionResult<Paged<ParticipationViewPoco>>> GetParticipations(int pageNumber, int pageSize)
        {
            var query = _participations.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get participations By IdeaId
        /// </summary>
        /// <returns></returns>
        [HttpGet("ideaId/{ideaId}")]
        [ProducesResponseType(typeof(Paged<ParticipationViewPoco>), 200)]
        public async Task<ActionResult<Paged<ParticipationViewPoco>>> GetParticipationsByIdeaId([FromRoute] Guid ideaId, int pageNumber, int pageSize)
        {
            var query = _participations.AsNoTracking().Where(q => q.IdeaId == ideaId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get participations By UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet("userId/{userId}")]
        [ProducesResponseType(typeof(Paged<ParticipationViewPoco>), 200)]
        public async Task<ActionResult<Paged<ParticipationViewPoco>>> GetParticipationsByUserId([FromRoute] long userId, int pageNumber, int pageSize)
        {
            var query = _participations.AsNoTracking().Where(q => q.UserId == userId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Participation By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ParticipationViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<ParticipationViewPoco>> GetParticipationInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participationInfo = await _participations.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (participationInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(participationInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Participation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participationPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutParticipationInfo([FromRoute] Guid id, [FromBody] ParticipationPoco participationPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var participationInfoEntity = await _participations.FirstOrDefaultAsync(q => q.Id == id);
            if (participationInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(participationPoco, participationInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Participation
        /// </summary>
        /// <param name="participationPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostParticipation([FromBody] ParticipationPoco participationPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participationInfoEntity = _mapper.Map<Participation>(participationPoco);
            _participations.Add(participationInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(participationInfoEntity.Id);
        }

        /// <summary>
        /// Delete Participation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteParticipation([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participationInfo = await _participations.FirstOrDefaultAsync(q => q.Id == id);
            if (participationInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(participationInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}