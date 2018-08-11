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
            foreach (var ideaOptionSetItemInfo in entities)
            {
                models.Add(ConvertToModel(ideaOptionSetItemInfo));
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

        private IdeaOptionSetItemViewPoco ConvertToModel(IdeaOptionSetItem ideaOptionSetItemInfo)
        {
            return new IdeaOptionSetItemViewPoco()
            {
                Id = ideaOptionSetItemInfo.Id,
                IdeaOptionSetItem = _mapper.Map<IdeaOptionSetItemPoco>(ideaOptionSetItemInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(ideaOptionSetItemInfo)
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
        public async Task<ActionResult<IdeaOptionSetItemViewPoco>> GetIdeaOptionSetItemInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaOptionSetItemInfo = await _ideaOptionSetItems.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (ideaOptionSetItemInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(ideaOptionSetItemInfo);
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
        public async Task<ActionResult<Guid>> PutIdeaOptionSetItemInfo([FromRoute] Guid id, [FromBody] IdeaOptionSetItemPoco ideaOptionSetItemPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var ideaOptionSetItemInfoEntity = await _ideaOptionSetItems.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaOptionSetItemInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(ideaOptionSetItemPoco, ideaOptionSetItemInfoEntity);

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

            var ideaOptionSetItemInfoEntity = _mapper.Map<IdeaOptionSetItem>(ideaOptionSetItemPoco);
            _ideaOptionSetItems.Add(ideaOptionSetItemInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(ideaOptionSetItemInfoEntity.Id);
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

            var ideaOptionSetItemInfo = await _ideaOptionSetItems.FirstOrDefaultAsync(q => q.Id == id);
            if (ideaOptionSetItemInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(ideaOptionSetItemInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}