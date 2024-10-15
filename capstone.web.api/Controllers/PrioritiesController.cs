using capstone.web.api.Data;
using capstone.web.api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capstone.web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioritiesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public PrioritiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Priorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Priority>>> GetCategories()
        {
            return await _context.Priorities.Where(a => !a.IsDeleted).ToListAsync();
        }

        // GET: api/Priorities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Priority>> GetCategory(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);

            if (priority == null)
            {
                NotFound();
            }

            return priority;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);

            if (priority == null)
            {
                NotFound();
            }

            priority.IsDeleted = true;
            //_context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<Priority>>> SearchPriorities(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return await _context.Priorities.ToListAsync(); // Return all if no term provided
            }

            var filteredPriorities = await _context.Priorities.Where(c => c.PriorityName.Contains(term) || c.Description.Contains(term)).Where(a => !a.IsDeleted).ToListAsync();

            return Ok(filteredPriorities);
        }

        // POST: api/Priorities
        [HttpPost]
        public async Task<ActionResult<Priority>> AddNewPriority(Priority priority)
        {
            if (priority == null)
            {
                return BadRequest("Priority cannot be null.");
            }

            _context.Priorities.Add(priority);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = priority.Id }, priority);
        }

        // PUT: api/Priorities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePriority(int id, Priority priority)
        {
            if (id != priority.PriorityId)
            {
                return BadRequest("Priority ID mismatch.");
            }

            var existingPriority = await _context.Priorities.FindAsync(id);
            if (existingPriority == null)
            {
                return NotFound();
            }

            existingPriority.PriorityName = priority.PriorityName;
            existingPriority.Description = priority.Description;
            existingPriority.PriorityLevel = priority.PriorityLevel;
            existingPriority.IsDeleted = priority.IsDeleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriorityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // or return Ok(existingPriority) if you want to return the updated entity
        }
        private bool PriorityExists(int id)
        {
            return _context.Priorities.Any(e => e.PriorityId == id);
        }



    }

}
