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
    public class DrawingsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DrawingsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/drawings/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<string>> GetDrawing(int userId)
        {
            var drawing = await _db.Drawings
                .OrderByDescending(d => d.UpdatedAt)
                .FirstOrDefaultAsync();

            if (drawing == null) return NotFound("No drawing found");

            return Ok(drawing.DrawingData);
        }

        // POST: api/drawings/save
        [HttpPost("save")]
        public async Task<IActionResult> SaveDrawing([FromBody] Drawing drawing)
        {
            if (string.IsNullOrEmpty(drawing.DrawingData))
                return BadRequest("Drawing data cannot be empty");

            drawing.UpdatedAt = DateTime.Now;

            _db.Drawings.Add(drawing);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
