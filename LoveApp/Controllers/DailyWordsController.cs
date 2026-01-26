using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- THIS

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DailyWordsController : ControllerBase
{
    private readonly AppDbContext _context;
    public DailyWordsController(AppDbContext context) => _context = context;

    // Get all words for a user (sender or receiver)
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<DailyWordDto>>> GetWordsForUser(int userId)
    {
        var words = await _context.DailyWords
            .Where(w => w.SenderId == userId || w.ReceiverId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();

        return words.Select(w => new DailyWordDto
        {
            Id = w.Id,
            Word = w.Word,
            Meaning = w.Meaning,
            SenderId = w.SenderId,
            ReceiverId = w.ReceiverId,
            Learned = w.Learned,
            CreatedAt = w.CreatedAt
        }).ToList();
    }

    // Add new word
    [HttpPost]
    public async Task<ActionResult<DailyWordDto>> AddWord(DailyWordDto wordDto)
    {
        var word = new DailyWords
        {
            Word = wordDto.Word,
            Meaning = wordDto.Meaning,
            SenderId = wordDto.SenderId,
            ReceiverId = wordDto.ReceiverId
        };

        _context.DailyWords.Add(word);
        await _context.SaveChangesAsync();

        wordDto.Id = word.Id;
        wordDto.CreatedAt = word.CreatedAt;
        return Ok(wordDto);
    }

    // Toggle Learned (only receiver can do it)
    [HttpPut("{id}/learned/{userId}")]
    public async Task<ActionResult> ToggleLearned(int id, int userId)
    {
        var word = await _context.DailyWords.FindAsync(id);
        if (word == null) return NotFound();

        if (word.ReceiverId != userId)
            return Forbid("Only the receiver can mark this word as learned.");

        word.Learned = !word.Learned;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
