using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EventTypes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.EventTypes.ToListAsync();
            return Ok(events);
        }

        // GET: api/EventTypes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventType = await _context.EventTypes.FindAsync(id);

            if (eventType == null)
            {
                return NotFound();
            }

            return Ok(eventType);
        }
    }
}