using capstone.web.api.Data;
using capstone.web.api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capstone.web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ToDoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDos()
        {
            return await _context.ToDos.ToListAsync();
        }

        // GET: api/ToDo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDo(int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }

            return toDo;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var todo = await _context.ToDos.FindAsync(id);

            if (todo == null)
            {
                NotFound();
            }

            todo.IsDeleted = true;
            //_context.ToDo.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }

        // PUT: api/ToDos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(int id, ToDo todo)
        {
            if (id != todo.ToDoId)
            {
                return BadRequest("ToDo ID mismatch.");
            }

            var existingToDo = await _context.ToDos.FindAsync(id);
            if (existingToDo == null)
            {
                return NotFound();
            }

            existingToDo.Name = todo.Name;
            existingToDo.Description = todo.Description;
            existingToDo.IsDeleted = todo.IsDeleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
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
        private bool ToDoExists(int id)
        {
            return _context.ToDos.Any(e => e.ToDoId == id);
        }
    }
}
