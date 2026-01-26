using Microsoft.AspNetCore.Mvc;
using LoveApp.Data; // your DbContext namespace
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LoveApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/home/notes
        [HttpGet("notes")]
        public IActionResult GetLatestNotes()
        {
            var notes = _dbContext.Notes
                .OrderByDescending(n => n.CreatedAt)
                .Take(3)
                .Select(n => new
                {
                    user = n.UserId == 1 ? "You" : "Partner",
                    message = n.Message
                })
                .ToList();

            return Ok(notes);
        }

        // GET: api/home/visits
        [HttpGet("nextvisit")]
        public IActionResult GetNextVisit()
        {
            var todayUtc = DateTime.Today.ToUniversalTime(); // converts local midnight to UTC

            // Find the next visit today or in the future
            var nextVisit = _dbContext.Visits
     .Where(v => v.StartDate >= todayUtc)
     .OrderBy(v => v.StartDate)
     .Select(v => new
     {
         v.StartDate,
         v.EndDate,
         v.City,
         v.Note
     })
     .FirstOrDefault();


            if (nextVisit == null)
                return NotFound("No upcoming visits");

            return Ok(nextVisit);
        }
    }
}
