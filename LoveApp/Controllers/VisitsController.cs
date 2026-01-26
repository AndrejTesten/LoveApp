using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoveApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public VisitsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/visits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisits()
        {
            return await _db.Visits.OrderBy(v => v.StartDate).ToListAsync();
        }

        // GET: api/visits/next
        [HttpGet("next")]
        public async Task<ActionResult<Visit>> GetNextVisit()
        {
            var today = DateTime.Today;
            var next = await _db.Visits
                .Where(v => v.StartDate >= today)
                .OrderBy(v => v.StartDate)
                .FirstOrDefaultAsync();

            if (next == null) return NotFound("No upcoming visits");

            return next;
        }

        // POST: api/visits
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult<Visit>> AddVisit([FromBody] Visit visit)
        {
            if (visit.EndDate < visit.StartDate)
                return BadRequest("End date cannot be before start date.");

            if (visit.StartDate < DateTime.Today)
                return BadRequest("Start date cannot be in the past.");

            _db.Visits.Add(visit);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVisits), new { id = visit.Id }, visit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisit(int id, [FromBody] Visit visit)
        {
            if (id != visit.Id) return BadRequest("ID mismatch");

            if (visit.EndDate < visit.StartDate)
                return BadRequest("End date cannot be before start date.");

            var existing = await _db.Visits.FindAsync(id);
            if (existing == null) return NotFound();

            existing.StartDate = visit.StartDate;
            existing.EndDate = visit.EndDate;
            existing.City = visit.City;
            existing.Note = visit.Note;

            await _db.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/visits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var visit = await _db.Visits.FindAsync(id);
            if (visit == null) return NotFound();

            _db.Visits.Remove(visit);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
