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
using Mizekar.Micro.Idea.Models.Services;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// services Management - مدیریت سرویس ها و خدمات
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Services", Name = "Services", Description = "Services Management - مدیریت سرویس ها و خدمات")]
    public class ServicesController : ControllerBase
    {
        private readonly DbSet<Service> _services;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ServicesController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _services = _context.Services;
        }

        private async Task<Paged<ServiceViewPoco>> ToPaged(IQueryable<Service> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<ServiceViewPoco>();
            foreach (var serviceInfo in entities)
            {
                models.Add(ConvertToModel(serviceInfo));
            }

            var resultPaged = new Paged<ServiceViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private ServiceViewPoco ConvertToModel(Service serviceInfo)
        {
            return new ServiceViewPoco()
            {
                Id = serviceInfo.Id,
                Service = _mapper.Map<ServicePoco>(serviceInfo),
                RelatedIdeasCount = serviceInfo.Ideas.Count,
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(serviceInfo)
            };
        }

        /// <summary>
        /// Get services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<ServiceViewPoco>), 200)]
        public async Task<ActionResult<Paged<ServiceViewPoco>>> GetServices(int pageNumber, int pageSize)
        {
            var query = _services
                .Include(i => i.Ideas)
                .OrderByDescending(o => o.EndDate)
                .AsNoTracking()
                .AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Service By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<ServiceViewPoco>> GetService([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceInfo = await _services
                .Include(i=>i.Ideas)
                .AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (serviceInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(serviceInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="servicePoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutServiceInfo([FromRoute] Guid id, [FromBody] ServicePoco servicePoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var serviceInfoEntity = await _services.FirstOrDefaultAsync(q => q.Id == id);
            if (serviceInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(servicePoco, serviceInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Service
        /// </summary>
        /// <param name="servicePoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostService([FromBody] ServicePoco servicePoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceInfoEntity = _mapper.Map<Service>(servicePoco);
            _services.Add(serviceInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(serviceInfoEntity.Id);
        }

        /// <summary>
        /// Delete Service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteService([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceInfo = await _services.FirstOrDefaultAsync(q => q.Id == id);
            if (serviceInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(serviceInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}