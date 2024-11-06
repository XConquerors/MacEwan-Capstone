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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ToDo>>> SearchToDos(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var results = await _context.ToDos
                .Where(t => (t.Name != null && t.Name.Contains(query)) ||
                             (t.Description != null && t.Description.Contains(query)))
                .ToListAsync();

            return results;
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> AddToDo(ToDo newToDo)
        {
            if (newToDo == null)
            {
                return BadRequest("ToDo item cannot be null.");
            }

           
            if (string.IsNullOrWhiteSpace(newToDo.Name))
            {
                return BadRequest("Name is required.");
            }

           
            _context.ToDos.Add(newToDo);
            await _context.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetToDo), new { id = newToDo.ToDoId }, newToDo);
        }
    }
}
