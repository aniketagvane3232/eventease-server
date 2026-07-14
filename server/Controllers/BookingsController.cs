using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using server.Data;
using server.DTOs;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings (Admin only - view all bookings)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.Bookings
                .Include(b => b.EventType)
                .Include(b => b.Package)
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: api/Bookings/user/1 (logged-in users can only view their own bookings)
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");

            if (loggedInUserId != userId && !isAdmin)
            {
                return Forbid();
            }

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.EventType)
                .Include(b => b.Package)
                .ToListAsync();

            return Ok(bookings);
        }

        // POST: api/Bookings (any logged-in user can create their own booking)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Booking booking)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Ignore whatever UserId the client sent - always use the logged-in user's own ID
            booking.UserId = loggedInUserId;
            booking.CreatedAt = DateTime.Now;
            booking.Status = "Pending";

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Booking Created Successfully"
            });
        }

        // PUT: api/Bookings/5/status (Admin only - approve/reject bookings)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusDto dto)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            var validStatuses = new[] { "Pending", "Approved", "Rejected" };
            if (!validStatuses.Contains(dto.Status))
            {
                return BadRequest(new { message = "Invalid status. Must be Pending, Approved, or Rejected." });
            }

            booking.Status = dto.Status;
            booking.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking status updated.", status = booking.Status });
        }
    }
}