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
    /// Idea Assessment OptionSet Items Management - مدیریت گزینه های ارزیابی
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "IdeaAssessmentOptionSetItems", Name = "IdeaAssessmentOptionSetItems", Description = "Idea Assessment OptionSet Items Management - مدیریت گزینه های ارزیابی")]
    public class IdeaAssessmentOptionSetItemsController : ControllerBase
    {
        private readonly DbSet<IdeaAssessmentOptionSetItem> _ideaAssessmentOptionSetItems;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public IdeaAssessmentOptionSetItemsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _ideaAssessmentOptionSetItems = _context.IdeaAssessmentOptionSetItems;
        }

        private async Task<Paged<IdeaAssessmentOptionSetItemViewPoco>> ToPaged(IQueryable<IdeaAssessmentOptionSetItem> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<IdeaAssessmentOptionSetItemViewPoco>();
            foreach (var ideaAssessmentOptionSetItem in entities)
            {
                models.Add(ConvertToModel(ideaAssessmentOptionSetItem));
            }

            var resultPaged = new Paged<IdeaAssessmentOptionSetItemViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private IdeaAssessmentOptionSetItemViewPoco ConvertToModel(IdeaAssessmentOptionSetItem ideaAssessmentOptionSetItem)
        {
            return new IdeaAssessmentOptionSetItemViewPoco()
            {
                Id = ideaAssessmentOptionSetItem.Id,
                IdeaAssessmentOptionSetItem = _mapper.Map<IdeaAssessmentOptionSetItemPoco>(ideaAssessmentOptionSetItem),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaAssessmentOptionSetItem)
            };
        }

        /// <summary>
        /// Get Idea Assessment OptionSet Items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaAssessmentOptionSetItemViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaAssessmentOptionSetItemViewPoco>>> GetIdeaAssessmentOptionSetItems(int pageNumber, int pageSize)
        {
            var query = _ideaAssessmentOptionSetItems.AsNoTracking()
                .OrderBy(o => o.Order)
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Idea Assessment OptionSet Items by optionSetId
        /// </summary>
        /// <returns></returns>
        [HttpGet("OptionSetId/{optionSetId}")]
        [ProducesResponseType(typeof(Paged<IdeaAssessmentOptionSetItemViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaAssessmentOptionSetItemViewPoco>>> GetIdeaAssessmentOptionSetItemsBySetId([FromRoute] Guid optionSetId, int pageNumber, int pageSize)
        {
            var query = _ideaAssessmentOptionSetItems.AsNoTracking()
                .Include(i=>i.IdeaAssessmentOptionSet)
                .Where(q => q.IdeaAssessmentOptionSetId == optionSetId)
                .OrderBy(o => o.Order)
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Idea Assessment OptionSet Item By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaAssessmentOptionSetItemViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<IdeaAssessmentOptionSetItemViewPoco>> GetIdeaAssessmentOptionSetItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAssessmentOptionSetItem = await _ideaAssessmentOptionSetItems.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaAssessmentOptionSetItem == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaAssessmentOptionSetItem);
            return Ok(poco);
        }

        /// <summary>
        /// Update Idea Assessment OptionSet Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ideaAssessmentOptionSetItemPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaAssessmentOptionSetItem([FromRoute] Guid id, [FromBody] IdeaAssessmentOptionSetItemPoco ideaAssessmentOptionSetItemPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaAssessmentOptionSetItemEntity = await _ideaAssessmentOptionSetItems.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaAssessmentOptionSetItemEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaAssessmentOptionSetItemPoco, ideaAssessmentOptionSetItemEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Idea Assessment OptionSet Item
        /// </summary>
        /// <param name="ideaAssessmentOptionSetItemPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostIdeaAssessmentOptionSetItem([FromBody] IdeaAssessmentOptionSetItemPoco ideaAssessmentOptionSetItemPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAssessmentOptionSetItemEntity = _mapper.Map<IdeaAssessmentOptionSetItem>(ideaAssessmentOptionSetItemPoco);
            _ideaAssessmentOptionSetItems.Add(ideaAssessmentOptionSetItemEntity);
            await _context.SaveChangesAsync();

            return Ok(ideaAssessmentOptionSetItemEntity.Id);
        }

        /// <summary>
        /// Delete Idea Assessment OptionSet Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteIdeaAssessmentOptionSetItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAssessmentOptionSetItem = await _ideaAssessmentOptionSetItems.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaAssessmentOptionSetItem == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaAssessmentOptionSetItem);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}