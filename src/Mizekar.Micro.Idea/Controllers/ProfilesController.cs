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
using Mizekar.Micro.Idea.Models.Profiles;
using NSwag.Annotations;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    /// Profiles Management - مدیریت پروفایل
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [SwaggerTag(name: "Profiles", Name = "Profiles", Description = "Profiles Management - مدیریت پروفایل")]
    public class ProfilesController : ControllerBase
    {
        private readonly DbSet<Data.Entities.Functional.Profile> _profiles;
        private readonly IdeaDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ProfilesController(IdeaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _profiles = _context.Profiles;
        }

        private async Task<Paged<ProfileViewPoco>> ToPaged(IQueryable<Data.Entities.Functional.Profile> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var models = new List<ProfileViewPoco>();
            foreach (var profileInfo in entities)
            {
                models.Add(ConvertToModel(profileInfo));
            }

            var resultPaged = new Paged<ProfileViewPoco>()
            {
                Items = models,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesCount = 0
            };
            return resultPaged;
        }

        private ProfileViewPoco ConvertToModel(Data.Entities.Functional.Profile profileInfo)
        {
            return new ProfileViewPoco()
            {
                Id = profileInfo.Id,
                Profile = _mapper.Map<ProfilePoco>(profileInfo),
                BusinessBaseInfo = _mapper.Map<BusinessBaseInfo>(profileInfo)
            };
        }

        /// <summary>
        /// Get profiles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<ProfileViewPoco>), 200)]
        public async Task<ActionResult<Paged<ProfileViewPoco>>> GetProfiles(int pageNumber, int pageSize)
        {
            var query = _profiles.AsNoTracking().AsQueryable();
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get profiles
        /// </summary>
        /// <returns></returns>
        [HttpGet("experts")]
        [ProducesResponseType(typeof(Paged<ProfileViewPoco>), 200)]
        public async Task<ActionResult<Paged<ProfileViewPoco>>> GetExpertProfiles(int pageNumber, int pageSize)
        {
            var query = _profiles.AsNoTracking().Where(q => q.IsExpertUser == true);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get profiles
        /// </summary>
        /// <returns></returns>
        [HttpGet("admins")]
        [ProducesResponseType(typeof(Paged<ProfileViewPoco>), 200)]
        public async Task<ActionResult<Paged<ProfileViewPoco>>> GetSuperAdminProfiles(int pageNumber, int pageSize)
        {
            var query = _profiles.AsNoTracking().Where(q => q.IsSuperAdmin == true);
            var resultPaged = await ToPaged(query, pageNumber, pageSize);
            return Ok(resultPaged);
        }

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProfileViewPoco), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<ProfileViewPoco>> GetProfileInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileInfo = await _profiles.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (profileInfo == null)
            {
                return NotFound(id);
            }

            var poco = ConvertToModel(profileInfo);
            return Ok(poco);
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profilePoco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> PutProfileInfo([FromRoute] Guid id, [FromBody] ProfilePoco profilePoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var profileInfoEntity = await _profiles.FirstOrDefaultAsync(q => q.Id == id);
            if (profileInfoEntity == null)
            {
                return NotFound(id);
            }

            _mapper.Map(profilePoco, profileInfoEntity);

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        /// <summary>
        /// Create Profile
        /// </summary>
        /// <param name="profilePoco"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<Guid>> PostProfile([FromBody] ProfilePoco profilePoco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileInfoEntity = _mapper.Map<Data.Entities.Functional.Profile>(profilePoco);
            _profiles.Add(profileInfoEntity);
            await _context.SaveChangesAsync();

            return Ok(profileInfoEntity.Id);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<ActionResult<Guid>> DeleteProfile([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileInfo = await _profiles.FirstOrDefaultAsync(q => q.Id == id);
            if (profileInfo == null)
            {
                return NotFound(id);
            }
            MarkAsDelete(profileInfo);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private void MarkAsDelete(IBusinessBaseEntity businessBaseEntity)
        {
            businessBaseEntity.IsDeleted = true;
        }
    }
}