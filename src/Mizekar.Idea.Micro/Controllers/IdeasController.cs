using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Idea.Micro.Data;
using Mizekar.Idea.Micro.Data.Entities;
using Mizekar.Idea.Micro.Models.Ideas;

namespace Mizekar.Idea.Micro.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IdeaDbContext _context;

        public IdeasController(IdeaDbContext context)
        {
            _context = context;
        }

        #region Map

        public IdeaView ConvertToView(IdeaInfo entity)
        {
            return AutoMapper.Mapper.Map<IdeaInfo, IdeaView>(entity);
        }

        #endregion

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdeaView>), 200)]
        public async Task<ActionResult<IEnumerable<IdeaView>>> GetIdeaInfos()
        {
            var entities = _context.IdeaInfos.ToList();
            return entities.Select(ideaInfo => ConvertToView(ideaInfo)).ToList();
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaView), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<ActionResult<IdeaView>> GetIdeaInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfo = await _context.IdeaInfos.FindAsync(id);

            if (ideaInfo == null)
            {
                return NotFound();
            }


            return Ok(ideaInfo);
        }

        // PUT: api/IdeaInfoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdeaInfo([FromRoute] Guid id, [FromBody] IdeaInfo ideaInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ideaInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(ideaInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/IdeaInfoes
        [HttpPost]
        public async Task<IActionResult> PostIdeaInfo([FromBody] IdeaInfo ideaInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.IdeaInfos.Add(ideaInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IdeaInfoExists(ideaInfo.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIdeaInfo", new { id = ideaInfo.Id }, ideaInfo);
        }

        // DELETE: api/IdeaInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdeaInfo([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaInfo = await _context.IdeaInfos.FindAsync(id);
            if (ideaInfo == null)
            {
                return NotFound();
            }

            _context.IdeaInfos.Remove(ideaInfo);
            await _context.SaveChangesAsync();

            return Ok(ideaInfo);
        }

        private bool IdeaInfoExists(Guid id)
        {
            return _context.IdeaInfos.Any(e => e.Id == id);
        }
    }
}