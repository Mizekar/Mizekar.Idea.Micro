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
using Mizekar.Micro.Idea.Models.Similar;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// similarIdeas Management - مدیریت ایده های مشابه
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "SimilarIdeas", Name = "SimilarIdeas", Description = "SimilarIdeas Management - مدیریت ایده های مشابه")]
    public class SimilarIdeasController : ControllerBase
    {
        private readonly DbSet<SimilarIdea> _similarIdeas;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SimilarIdeasController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _similarIdeas = _context.SimilarIdeas;
        }

        private async Task<Paged<SimilarIdeaViewPoco>> ToPaged(IQueryable<SimilarIdea> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<SimilarIdeaViewPoco>();
            foreach (var similarIdeaInfo in entities)
            {
                models.Add(ConvertToModel(similarIdeaInfo));
            }

            var resultPaged = new Paged<SimilarIdeaViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private SimilarIdeaViewPoco ConvertToModel(SimilarIdea similarIdeaInfo)
        {
            return new SimilarIdeaViewPoco()
            {
                Id = similarIdeaInfo.Id,
                SimilarIdea = _mapper.Map<SimilarIdeaPoco>(similarIdeaInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(similarIdeaInfo)
            };
        }

        /// <summary>
        /// Get similarIdeas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<SimilarIdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<SimilarIdeaViewPoco>>> GetSimilarIdeas(int pageNumber, int pageSize)
        {
            var query = _similarIdeas.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get similarIdeas
        /// </summary>
        /// <returns></returns>
        [HttpGet("ideaId/{ideaId}")]
        [ProducesResponseType(typeof(Paged<SimilarIdeaViewPoco>), 200)]
        public async Task<ActionResult<Paged<SimilarIdeaViewPoco>>> GetSimilarIdeasByIdeaId([FromRoute] Guid ideaId, int pageNumber, int pageSize)
        {
            var query = _similarIdeas.AsNoTracking().Where(q => q.IdeaId == ideaId);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get SimilarIdea By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SimilarIdeaViewPoco), 200)]
        public async Task<ActionResult<SimilarIdeaViewPoco>> GetSimilarIdeaInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var similarIdeaInfo = await _similarIdeas.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (similarIdeaInfo == null)
            {
                return NotFound();
            }

            var poco = ConvertToModel(similarIdeaInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update SimilarIdea
        /// </summary>
        /// <param name="id"></param>
        /// <param name="similarIdeaPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> PutSimilarIdeaInfo([FromRoute] Guid id, [FromBody] SimilarIdeaPoco similarIdeaPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var similarIdeaInfoEntity = await _similarIdeas.FirstOrDefaultAsync(q => q.Id == id);
            if (similarIdeaInfoEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(similarIdeaPoco, similarIdeaInfoEntity);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create SimilarIdea
        /// </summary>
        /// <param name="similarIdeaPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> PostSimilarIdea([FromBody] SimilarIdeaPoco similarIdeaPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var similarIdeaInfoEntity = _mapper.Map<SimilarIdea>(similarIdeaPoco);
            _similarIdeas.Add(similarIdeaInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(similarIdeaInfoEntity.Id);
        }

        /// <summary>
        /// Delete SimilarIdea
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteSimilarIdea([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var similarIdeaInfo = await _similarIdeas.FirstOrDefaultAsync(q => q.Id == id);
            if (similarIdeaInfo == null)
            {
                return NotFound();
            }
            MarkAsDelete(similarIdeaInfo);
            await _context.SaveChangesAsync();

            return Ok(similarIdeaInfo);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}