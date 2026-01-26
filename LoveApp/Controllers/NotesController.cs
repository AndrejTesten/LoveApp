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
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public NotesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _db.Notes.OrderBy(n => n.CreatedAt).ToListAsync();
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> AddNote([FromBody] NoteDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest("Message cannot be empty.");

            var today = DateTime.Now.Date; // local server day

            // Check if the user already wrote a note today
            bool alreadyExists = await _db.Notes
                .AnyAsync(n => n.UserId == dto.UserId && n.CreatedAt.Date == today);

            if (alreadyExists)
                return BadRequest("You can only write one note per day 💌");

            var note = new Note
            {
                UserId = dto.UserId,
                Message = dto.Message,
                CreatedAt = DateTime.Now
            };

            _db.Notes.Add(note);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, note);
        }
    }

    // DTO for creating a note
    public class NoteDto
    {
        public int UserId { get; set; }    // 1 = me, 2 = her
        public string Message { get; set; } = "";
    }
}
