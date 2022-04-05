using Glossary.Api.Data;
using Glossary.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Glossary.Api.Controllers
{
    /// <summary>
    /// API to manage glossary items
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GlossaryController : ControllerBase
    {
        private readonly GlossaryDbContext _context;
        private readonly ILogger<GlossaryController> _logger;

        public GlossaryController(GlossaryDbContext context, ILogger<GlossaryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Delete a glossary item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var glossaryItem = await _context.GlossaryItems.FindAsync(id);

            if (glossaryItem == null)
            {
                return NotFound();
            }

            _context.GlossaryItems.Remove(glossaryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Get the list of glossary items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<GlossaryItem>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GlossaryItem>>> GetAsync()
        {
            var glossary = await _context.GlossaryItems.AsNoTracking().OrderBy(x => x.Term.ToUpper()).ToListAsync();
            return Ok(glossary);
        }
        
        /// <summary>
        /// Get a glossary item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ActionName(nameof(GetAsync))]
        [ProducesResponseType(typeof(GlossaryItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GlossaryItem>> GetAsync(int id)
        {
            var glossaryItem = await _context.GlossaryItems.FindAsync(id);

            if (glossaryItem == null)
            {
                return NotFound();
            }

            return Ok(glossaryItem);
        }
        
        /// <summary>
        /// Create a glossary item
        /// </summary>
        /// <param name="glossaryItem"></param>
        /// <remarks>Only creates the glossary item, if the term doesn't exist</remarks>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GlossaryItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PostAsync(GlossaryItem glossaryItem)
        {
            // Further check to ensure we aren't overriding a term that already exists
            var termExists = await _context.GlossaryItems
                .AnyAsync(x => x.Term.Equals(glossaryItem.Term));

            if (termExists)
            {
                return Conflict(new { message = $"An existing record with the term '{glossaryItem.Term}' was already found" });
            }

            var newGlossaryItem = new GlossaryItem
            {
                Definition = glossaryItem.Definition,
                Term = glossaryItem.Term
            };

            _context.Add(newGlossaryItem);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetAsync), new { id = newGlossaryItem.Id}, newGlossaryItem);
        }
        
        /// <summary>
        /// Update a glossary Item
        /// </summary>
        /// <param name="glossaryItem"></param>
        /// <remarks>Only updates if the term doesn't already exist</remarks>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(GlossaryItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PutAsync(GlossaryItem glossaryItem)
        {
            var existingGlossaryItem = await _context.GlossaryItems.FindAsync(glossaryItem.Id);
            if (existingGlossaryItem == null)
            {
                return NotFound("Glossary item cannot be updated because it does not exist");
            }

            // Further check to ensure we aren't overriding a term that already exists
            if (!existingGlossaryItem.Term.Equals(glossaryItem.Term))
            {
                var termExists = await _context.GlossaryItems.AnyAsync(x => x.Term.Equals(glossaryItem.Term));
                if (termExists)
                {
                    return Conflict(new { message = $"An existing record with the term '{glossaryItem.Term}' was already found" });
                }
            }
            
            existingGlossaryItem.Term = glossaryItem.Term;
            existingGlossaryItem.Definition = glossaryItem.Definition;
            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}