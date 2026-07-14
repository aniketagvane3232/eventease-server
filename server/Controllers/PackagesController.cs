using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PackagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Packages (public)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var packages = await _context.Packages
                .Include(p => p.EventType)
                .Include(p => p.Images)
                .ToListAsync();

            return Ok(packages);
        }

        // GET: api/Packages/trending (public)
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrending()
        {
            var packages = await _context.Packages
                .Where(p => p.IsTrending)
                .Include(p => p.EventType)
                .Include(p => p.Images)
                .ToListAsync();

            return Ok(packages);
        }

        // GET: api/Packages/event/1 (public)
        [HttpGet("event/{eventTypeId}")]
        public async Task<IActionResult> GetByEvent(int eventTypeId)
        {
            var packages = await _context.Packages
                .Where(p => p.EventTypeId == eventTypeId)
                .Include(p => p.Images)
                .ToListAsync();

            return Ok(packages);
        }

        // GET: api/Packages/5 (public - single package detail, with full gallery)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var package = await _context.Packages
                .Include(p => p.EventType)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            return Ok(package);
        }

        // POST: api/Packages (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PackageDto dto)
        {
            var package = new Package
            {
                EventTypeId = dto.EventTypeId,
                PackageName = dto.PackageName,
                Price = dto.Price,
                Description = dto.Description,
                IsTrending = dto.IsTrending,
                Images = dto.ImageUrls
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .Select(url => new PackageImage { ImageUrl = url })
                    .ToList()
            };

            _context.Packages.Add(package);
            await _context.SaveChangesAsync();

            return Ok(package);
        }

        // PUT: api/Packages/5 (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, PackageDto dto)
        {
            var package = await _context.Packages
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            package.EventTypeId = dto.EventTypeId;
            package.PackageName = dto.PackageName;
            package.Price = dto.Price;
            package.Description = dto.Description;
            package.IsTrending = dto.IsTrending;

            // Replace all images with the new list
            _context.PackageImages.RemoveRange(package.Images!);
            package.Images = dto.ImageUrls
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select(url => new PackageImage { ImageUrl = url })
                .ToList();

            await _context.SaveChangesAsync();

            return Ok(package);
        }

        // DELETE: api/Packages/5 (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}