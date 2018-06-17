using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Core.Model.Api.Response;
using Mizekar.Micro.Idea.Data;
using Mizekar.Micro.Idea.Data.Entities;
using Mizekar.Micro.Idea.Models.IdeaAtachements;
using NSwag;

namespace Mizekar.Micro.Idea.Controllers
{
    /// <summary>
    ///  Idea Atachements api
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AtachementsController : ControllerBase
    {
        private readonly IdeaDbContext _context;

        public AtachementsController(IdeaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IdeaAtachementView),200)]
        [ProducesResponseType(typeof(void),400)]
        [ProducesResponseType(typeof(void),404)]
        public async Task<ActionResult<IdeaAtachementView>> GetIdeaAtachement([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAtachement = await _context.IdeaAtachements.FindAsync(id);

            if (ideaAtachement == null)
            {
                return NotFound();
            }
            
            var result = AutoMapper.Mapper.Map<IdeaAtachement, IdeaAtachementView>(ideaAtachement);
            return Ok(result);
        }

        /// <summary>
        /// Update Att
        /// </summary>
        /// <param name="id">idddd</param>
        /// <param name="ideaAtachement">modellll</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdeaAtachement([FromRoute] Guid id, [FromBody] IdeaAtachement ideaAtachement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ideaAtachement.Id)
            {
                return BadRequest();
            }

            _context.Entry(ideaAtachement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaAtachementExists(id))
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

        /// <summary>
        /// New Att
        /// </summary>
        /// <param name="ideaAtachement"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostIdeaAtachement([FromBody] IdeaAtachement ideaAtachement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.IdeaAtachements.Add(ideaAtachement);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IdeaAtachementExists(ideaAtachement.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIdeaAtachement", new { id = ideaAtachement.Id }, ideaAtachement);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdeaAtachement([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ideaAtachement = await _context.IdeaAtachements.FindAsync(id);
            if (ideaAtachement == null)
            {
                return NotFound();
            }

            _context.IdeaAtachements.Remove(ideaAtachement);
            await _context.SaveChangesAsync();

            return Ok(ideaAtachement);
        }

        private bool IdeaAtachementExists(Guid id)
        {
            return _context.IdeaAtachements.Any(e => e.Id == id);
        }
    }
}