using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var dashboard = new AdminDashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),

                TotalBookings = await _context.Bookings.CountAsync(),

                PendingBookings = await _context.Bookings
                    .CountAsync(x => x.Status == "Pending"),

                TotalRevenue = await _context.Bookings
                    .SumAsync(x => (decimal?)x.TotalAmount) ?? 0
            };

            return Ok(dashboard);
        }
    }
}