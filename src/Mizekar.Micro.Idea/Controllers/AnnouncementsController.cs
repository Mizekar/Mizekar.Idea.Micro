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
using Mizekar.Micro.Idea.Models.Announcements;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// announcements Management - مدیریت فراخوان
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Announcements", Name = "Announcements", Description = "Announcements Management - مدیریت فراخوان")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly DbSet<Announcement> _announcements;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AnnouncementsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _announcements = _context.Announcements;
        }

        private async Task<Paged<AnnouncementViewPoco>> ToPaged(IQueryable<Announcement> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<AnnouncementViewPoco>();
            foreach (var announcementInfo in entities)
            {
                models.Add(ConvertToModel(announcementInfo));
            }

            var resultPaged = new Paged<AnnouncementViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private AnnouncementViewPoco ConvertToModel(Announcement announcementInfo)
        {
            return new AnnouncementViewPoco()
            {
                Id = announcementInfo.Id,
                Announcement = _mapper.Map<AnnouncementPoco>(announcementInfo),
                RelatedIdeasCount = announcementInfo.Ideas.Count,
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(announcementInfo)
            };
        }

        /// <summary>
        /// Get announcements
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<AnnouncementViewPoco>), 200)]
        public async Task<ActionResult<Paged<AnnouncementViewPoco>>> GetAnnouncements(int pageNumber, int pageSize)
        {
            var query = _announcements
                .Include(i => i.Ideas)
                .OrderByDescending(o => o.EndDate)
                .AsNoTracking()
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Announcement By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AnnouncementViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<AnnouncementViewPoco>> GetAnnouncement([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var announcementInfo = await _announcements
                .Include(i=>i.Ideas)
                .AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (announcementInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(announcementInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Announcement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="announcementPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutAnnouncementInfo([FromRoute] Guid id, [FromBody] AnnouncementPoco announcementPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var announcementInfoEntity = await _announcements.FirstOrDefaultAsync(q => q.Id == id);
            if (announcementInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(announcementPoco, announcementInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Announcement
        /// </summary>
        /// <param name="announcementPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostAnnouncement([FromBody] AnnouncementPoco announcementPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var announcementInfoEntity = _mapper.Map<Announcement>(announcementPoco);
            _announcements.Add(announcementInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(announcementInfoEntity.Id);
        }

        /// <summary>
        /// Delete Announcement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteAnnouncement([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var announcementInfo = await _announcements.FirstOrDefaultAsync(q => q.Id == id);
            if (announcementInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(announcementInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}