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
    /// Idea OptionSetItems Management - مدیریت گزینه ها
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "IdeaOptionSetItems", Name = "IdeaOptionSetItems", Description = "Idea OptionSetItems Management - مدیریت گزینه ها")]
    public class IdeaOptionSetItemsController : ControllerBase
    {
        private readonly DbSet<IdeaOptionSetItem> _ideaOptionSetItems;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public IdeaOptionSetItemsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _ideaOptionSetItems = _context.IdeaOptionSetItems;
        }

        private async Task<Paged<IdeaOptionSetItemViewPoco>> ToPaged(IQueryable<IdeaOptionSetItem> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<IdeaOptionSetItemViewPoco>();
            foreach (var ideaOptionSetItem in entities)
            {
                models.Add(ConvertToModel(ideaOptionSetItem));
            }

            var resultPaged = new Paged<IdeaOptionSetItemViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private IdeaOptionSetItemViewPoco ConvertToModel(IdeaOptionSetItem ideaOptionSetItem)
        {
            return new IdeaOptionSetItemViewPoco()
            {
                Id = ideaOptionSetItem.Id,
                IdeaOptionSetItem = _mapper.Map<IdeaOptionSetItemPoco>(ideaOptionSetItem),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaOptionSetItem)
            };
        }

        /// <summary>
        /// Get ideaOptionSetItems
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<IdeaOptionSetItemViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaOptionSetItemViewPoco>>> GetIdeaOptionSetItems(int pageNumber, int pageSize)
        {
            var query = _ideaOptionSetItems.AsNoTracking()
                .OrderBy(o => o.Order)
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get ideaOptionSetItems by optionSetId
        /// </summary>
        /// <returns></returns>
        [HttpGet("OptionSetId/{optionSetId}")]
        [ProducesResponseType(typeof(Paged<IdeaOptionSetItemViewPoco>), 200)]
        public async Task<ActionResult<Paged<IdeaOptionSetItemViewPoco>>> GetIdeaOptionSetItemsBySetId([FromRoute] Guid optionSetId, int pageNumber, int pageSize)
        {
            var query = _ideaOptionSetItems.AsNoTracking()
                .Include(i=>i.IdeaOptionSet)
                .Where(q => q.IdeaOptionSetId == optionSetId)
                .OrderBy(o => o.Order)
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get IdeaOptionSetItem By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaOptionSetItemViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<IdeaOptionSetItemViewPoco>> GetIdeaOptionSetItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetItem = await _ideaOptionSetItems.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaOptionSetItem == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaOptionSetItem);
            return Ok(poco);
        }

        /// <summary>
        /// Update IdeaOptionSetItem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ideaOptionSetItemPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutIdeaOptionSetItem([FromRoute] Guid id, [FromBody] IdeaOptionSetItemPoco ideaOptionSetItemPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaOptionSetItemEntity = await _ideaOptionSetItems.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaOptionSetItemEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaOptionSetItemPoco, ideaOptionSetItemEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create IdeaOptionSetItem
        /// </summary>
        /// <param name="ideaOptionSetItemPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostIdeaOptionSetItem([FromBody] IdeaOptionSetItemPoco ideaOptionSetItemPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetItemEntity = _mapper.Map<IdeaOptionSetItem>(ideaOptionSetItemPoco);
            _ideaOptionSetItems.Add(ideaOptionSetItemEntity);
            await _context.SaveChangesAsync();

            return Ok(ideaOptionSetItemEntity.Id);
        }

        /// <summary>
        /// Delete IdeaOptionSetItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteIdeaOptionSetItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetItem = await _ideaOptionSetItems.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaOptionSetItem == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaOptionSetItem);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}