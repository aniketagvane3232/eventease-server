using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailableDatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvailableDatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AvailableDates (public - only future, available dates)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dates = await _context.AvailableDates
                .Where(d => d.Date >= DateTime.Today)
                .OrderBy(d => d.Date)
                .ToListAsync();

            return Ok(dates);
        }

        // GET: api/AvailableDates/check/2026-08-15 (public - check one specific date)
        [HttpGet("check/{date}")]
        public async Task<IActionResult> CheckDate(DateTime date)
        {
            var entry = await _context.AvailableDates
                .FirstOrDefaultAsync(d => d.Date.Date == date.Date);

            // If no record exists for a date, treat it as available by default
            var isAvailable = entry == null || entry.IsAvailable;

            return Ok(new { date = date.ToString("yyyy-MM-dd"), isAvailable });
        }

        // GET: api/AvailableDates/all (Admin only - see everything including past/blocked)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var dates = await _context.AvailableDates
                .OrderBy(d => d.Date)
                .ToListAsync();

            return Ok(dates);
        }

        // POST: api/AvailableDates (Admin only - block/set a date)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AvailableDate dto)
        {
            var existing = await _context.AvailableDates
                .FirstOrDefaultAsync(d => d.Date.Date == dto.Date.Date);

            if (existing != null)
            {
                existing.IsAvailable = dto.IsAvailable;
                await _context.SaveChangesAsync();
                return Ok(existing);
            }

            _context.AvailableDates.Add(dto);
            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        // DELETE: api/AvailableDates/5 (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.AvailableDates.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.AvailableDates.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}