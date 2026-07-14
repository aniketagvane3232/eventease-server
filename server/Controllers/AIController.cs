using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AIController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat(ChatRequest request)
        {
            var message = request.Message?.ToLower().Trim() ?? "";

            if (string.IsNullOrWhiteSpace(message))
            {
                return Ok(new ChatResponse { Reply = "Please type a question — for example, ask about our packages, prices, or event types." });
            }

            // Greeting
            if (ContainsAny(message, "hi", "hello", "hey"))
            {
                return Ok(new ChatResponse { Reply = "Hi! I'm the EventEase assistant. Ask me about our event types, packages, prices, or date availability." });
            }

            // Trending packages
            if (ContainsAny(message, "trending", "popular", "best"))
            {
                var trending = await _context.Packages
                    .Where(p => p.IsTrending)
                    .Include(p => p.EventType)
                    .ToListAsync();

                return Ok(new ChatResponse { Reply = FormatPackageList("Here are our trending packages:", trending) });
            }

            // Specific event type mentioned (Wedding, Birthday, Corporate, etc.)
            var eventTypes = await _context.EventTypes.ToListAsync();
            var matchedEventType = eventTypes.FirstOrDefault(e => message.Contains(e.Name.ToLower()));

            if (matchedEventType != null)
            {
                var packages = await _context.Packages
                    .Where(p => p.EventTypeId == matchedEventType.Id)
                    .Include(p => p.EventType)
                    .ToListAsync();

                if (packages.Any())
                {
                    return Ok(new ChatResponse
                    {
                        Reply = FormatPackageList($"Here are our packages for {matchedEventType.Name}:", packages)
                    });
                }

                return Ok(new ChatResponse
                {
                    Reply = $"{matchedEventType.Name} starts at ₹{matchedEventType.BasePrice}. {matchedEventType.Description}"
                });
            }

            // Package name mentioned (Silver, Gold, Platinum, etc.)
            var allPackages = await _context.Packages.Include(p => p.EventType).ToListAsync();
            var matchedPackage = allPackages.FirstOrDefault(p => message.Contains(p.PackageName.ToLower()));

            if (matchedPackage != null)
            {
                return Ok(new ChatResponse
                {
                    Reply = $"{matchedPackage.PackageName} ({matchedPackage.EventType?.Name}): ₹{matchedPackage.Price}. {matchedPackage.Description}"
                });
            }

            // General "packages" or "prices" question
            if (ContainsAny(message, "package", "price", "cost", "how much"))
            {
                return Ok(new ChatResponse { Reply = FormatPackageList("Here are all our packages:", allPackages) });
            }

            // Event types list
            if (ContainsAny(message, "event", "events", "what do you offer", "services"))
            {
                var sb = new StringBuilder("We offer the following event types:\n");
                foreach (var e in eventTypes)
                {
                    sb.AppendLine($"- {e.Name}: starting at ₹{e.BasePrice}");
                }
                return Ok(new ChatResponse { Reply = sb.ToString() });
            }

            // Date availability check
            if (ContainsAny(message, "available", "availability", "date", "book"))
            {
                var dateMatch = TryExtractDate(message);

                if (dateMatch.HasValue)
                {
                    var utcDate = DateTime.SpecifyKind(dateMatch.Value.Date, DateTimeKind.Utc);
                    var entry = await _context.AvailableDates.FirstOrDefaultAsync(d => d.Date == utcDate);
                    var isAvailable = entry == null || entry.IsAvailable;

                    return Ok(new ChatResponse
                    {
                        Reply = isAvailable
                            ? $"Yes, {dateMatch.Value:yyyy-MM-dd} is available for booking!"
                            : $"Sorry, {dateMatch.Value:yyyy-MM-dd} is not available. Please choose another date."
                    });
                }

                return Ok(new ChatResponse
                {
                    Reply = "All dates are available by default unless fully booked. Tell me a specific date (e.g. 2026-08-15) and I'll check it for you."
                });
            }

            // Fallback
            return Ok(new ChatResponse
            {
                Reply = "I can help with questions about our event types, packages, prices, and date availability. Try asking something like 'What wedding packages do you have?' or 'Is 2026-08-20 available?'"
            });
        }

        private bool ContainsAny(string message, params string[] keywords)
        {
            return keywords.Any(k => message.Contains(k));
        }

        private string FormatPackageList(string header, List<Package> packages)
        {
            if (!packages.Any())
            {
                return "Sorry, I couldn't find any packages matching that.";
            }

            var sb = new StringBuilder(header + "\n");
            foreach (var p in packages)
            {
                sb.AppendLine($"- {p.PackageName} ({p.EventType?.Name}): ₹{p.Price} - {p.Description}");
            }
            return sb.ToString();
        }

        private DateTime? TryExtractDate(string message)
        {
            // Looks for a date pattern like 2026-08-15 or 15/08/2026
            var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy" };
            var words = message.Split(' ', ',', '.', '?', '!');

            foreach (var word in words)
            {
                foreach (var format in formats)
                {
                    if (DateTime.TryParseExact(word, format, null, System.Globalization.DateTimeStyles.None, out var result))
                    {
                        return result;
                    }
                }

                if (DateTime.TryParse(word, out var generalResult))
                {
                    return generalResult;
                }
            }

            return null;
        }
    }
}