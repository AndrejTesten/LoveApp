using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoveApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MysteryController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;

        public MysteryController(AppDbContext db, IHttpContextAccessor httpContext)
        {
            _db = db;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Get the first message (always the same)
        /// </summary>
        [HttpGet("first-message")]
        public IActionResult GetFirstMessage()
        {
            return Ok(new
            {
                message = "Hello Laila, I am bringing you a message from Andrej!"
            });
        }

        /// <summary>
        /// Get the second message (random, no repeats, max one per day)
        /// </summary>
        [HttpGet("second-message")]
        public async Task<IActionResult> GetSecondMessage()
        {
            // Get current userId (from auth, fallback to 1 for testing)
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID missing in token");

            int userId = int.Parse(userIdClaim.Value);

            // Check if user already got a message today
            var today = DateTime.UtcNow.Date;
            var alreadyToday = await _db.SentMysteryMessages
                .Where(sm => sm.UserId == userId && sm.SentAt.Date == today)
                .AnyAsync();

            if (alreadyToday)
            {
                return Ok(new
                {
                    message = "Penguin is traveling to Slovenia for more messages 💌, he will be back soon."
                });
            }

            // All messages
            var allMessages = await _db.MysteryMessages.ToListAsync();

            // Messages already sent to this user
            var sentMessageIds = await _db.SentMysteryMessages
                .Where(sm => sm.UserId == userId)
                .Select(sm => sm.MysteryMessageId)
                .ToListAsync();

            // Filter messages not yet sent
            var availableMessages = allMessages
                .Where(m => !sentMessageIds.Contains(m.Id))
                .ToList();

            if (!availableMessages.Any())
            {
                return Ok(new
                {
                    message = "Penguin is traveling to Slovenia for more messages 💌, he will be back soon."
                });
            }

            // Pick a random one
            var rnd = new Random();
            var chosen = availableMessages[rnd.Next(availableMessages.Count)];

            // Record as sent
            _db.SentMysteryMessages.Add(new SentMysteryMessages
            {
                UserId = userId,
                MysteryMessageId = chosen.Id,
                SentAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            return Ok(new { message = chosen.Message });
        }
    }
}
