using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.Data.Entities;

namespace Mizekar.Micro.Idea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeaStatusController : ControllerBase
    {
        private readonly IdeaDbContext _context;

        public IdeaStatusController(IdeaDbContext context)
        {
            _context = context;
        }

        // GET: api/IdeaStatus
        [HttpGet]
        public IEnumerable<IdeaStatus> GetIdeaStatuses()
        {
            return _context.IdeaStatuses;
        }

        // GET: api/IdeaStatus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdeaStatus([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaStatus = await _context.IdeaStatuses.FindAsync(id);

            if (ideaStatus == null)
            {
                return NotFound();
            }

            return Ok(ideaStatus);
        }

        // PUT: api/IdeaStatus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdeaStatus([FromRoute] Guid id, [FromBody] IdeaStatus ideaStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ideaStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(ideaStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaStatusExists(id))
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

        // POST: api/IdeaStatus
        [HttpPost]
        public async Task<IActionResult> PostIdeaStatus([FromBody] IdeaStatus ideaStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.IdeaStatuses.Add(ideaStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IdeaStatusExists(ideaStatus.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIdeaStatus", new { id = ideaStatus.Id }, ideaStatus);
        }

        // DELETE: api/IdeaStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdeaStatus([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaStatus = await _context.IdeaStatuses.FindAsync(id);
            if (ideaStatus == null)
            {
                return NotFound();
            }

            _context.IdeaStatuses.Remove(ideaStatus);
            await _context.SaveChangesAsync();

            return Ok(ideaStatus);
        }

        private bool IdeaStatusExists(Guid id)
        {
            return _context.IdeaStatuses.Any(e => e.Id == id);
        }
    }
}