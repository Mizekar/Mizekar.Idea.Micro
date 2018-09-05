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
using Mizekar.Micro.Idea.Data.Entities.Functional;
using Mizekar.Micro.Idea.Models.Permissions;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// permissions Management - مدیریت دسترسی ها
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Permissions", Name = "Permissions", Description = "Permissions Management - مدیریت دسترسی ها")]
    public class PermissionsController : ControllerBase
    {
        private readonly DbSet<Permission> _permissions;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public PermissionsController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _permissions = _context.Permissions;
        }

        private async Task<Paged<PermissionViewPoco>> ToPaged(IQueryable<Permission> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<PermissionViewPoco>();
            foreach (var permissionInfo in entities)
            {
                models.Add(ConvertToModel(permissionInfo));
            }

            var resultPaged = new Paged<PermissionViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private PermissionViewPoco ConvertToModel(Permission permissionInfo)
        {
            return new PermissionViewPoco()
            {
                Id = permissionInfo.Id,
                Permission = _mapper.Map<PermissionPoco>(permissionInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(permissionInfo)
            };
        }

        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<PermissionViewPoco>), 200)]
        public async Task<ActionResult<Paged<PermissionViewPoco>>> GetPermissions(int pageNumber, int pageSize)
        {
            var query = _permissions.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Permissions By UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet("userId/{userId}")]
        [ProducesResponseType(typeof(List<PermissionViewPoco>), 200)]
        public async Task<ActionResult<List<PermissionViewPoco>>> GetPermissionsByUserId([FromRoute] long userId)
        {
            var query = await _context.PermissionOwners.Where(q => q.UserId == userId).Select(s => s.Permission).ToListAsync();
            var result = query.Select(ConvertToModel).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Update Permissions for UserId
        /// </summary>
        /// <returns></returns>
        [HttpPut("userId/{userId}")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<ActionResult<long>> SetPermissionsForUserId([FromRoute] long userId, Guid[] permissionIds)
        {
            if (permissionIds == null)
            {
                return BadRequest("permissions is null");
            };

            var currentPermissionOwners = await _context.PermissionOwners.Where(q => q.UserId == userId).ToListAsync();
            // remove none selected
            foreach (var currentPermission in currentPermissionOwners)
            {
                if (!permissionIds.Contains(currentPermission.PermissionId))
                {
                    currentPermission.IsDeleted = true;
                }
            }

            // add new relation
            foreach (var permissionId in permissionIds)
            {
                if (currentPermissionOwners.FirstOrDefault(f => f.PermissionId == permissionId) == null)
                {
                    var newPermissionOwner = new PermissionOwner()
                    {
                        PermissionId = permissionId,
                        UserId = userId
                    };
                    _context.Add(newPermissionOwner);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(userId);
        }

        /// <summary>
        /// Get Permission By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<PermissionViewPoco>> GetPermissionInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionInfo = await _permissions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (permissionInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(permissionInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissionPoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutPermissionInfo([FromRoute] Guid id, [FromBody] PermissionPoco permissionPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var permissionInfoEntity = await _permissions.FirstOrDefaultAsync(q => q.Id == id);
            if (permissionInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(permissionPoco, permissionInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Permission
        /// </summary>
        /// <param name="permissionPoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostPermission([FromBody] PermissionPoco permissionPoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionInfoEntity = _mapper.Map<Permission>(permissionPoco);
            _permissions.Add(permissionInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(permissionInfoEntity.Id);
        }

        /// <summary>
        /// Delete Permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeletePermission([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionInfo = await _permissions.FirstOrDefaultAsync(q => q.Id == id);
            if (permissionInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(permissionInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}