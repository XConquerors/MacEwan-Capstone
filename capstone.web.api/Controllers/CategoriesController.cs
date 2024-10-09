using capstone.web.api.Data;
using capstone.web.api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace capstone.web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await _context.Categories.Where(a => !a.IsDeleted).ToListAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>>GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                NotFound();
            }

            return category;
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            category.CategoryId = 0;
            category.IsDeleted = false;

            _context.Categories.Add(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (CategoryExists(category.CategoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw ex;
                }
            }

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId} , category);
        }

        private bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.CategoryId == categoryId);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] Category     category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest("Both the id's should match");
            }
            var existingCategory = _context.Categories.Find(id);

            if (category == null)
            {
                NotFound();
            }
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) { 
                if(CategoryExists(existingCategory.CategoryId)) { return Conflict(); } else { throw ex; }
            }

            return NoContent();

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                NotFound();
            }

            category.IsDeleted = true;
            //_context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent(); 
        }

        [HttpGet("search")]

        public async Task<ActionResult<IEnumerable<Category>>> SearchCategories(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return await _context.Categories.ToListAsync(); // Return all if no term provided
            }

            var filteredCategories = await _context.Categories.Where(c => c.CategoryName.Contains(term) || c.Description.Contains(term)).Where(a => !a.IsDeleted).ToListAsync();

            return Ok(filteredCategories);
        }
    }
}
