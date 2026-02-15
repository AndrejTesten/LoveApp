using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public QuestionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysQuestion()
    {
        var startDate = new DateTime(2025, 12, 05); // example start day
        var today = DateTime.UtcNow.Date;
        var dayNumber = (today - startDate.Date).Days + 1;

        var question = await _db.Questions.FirstOrDefaultAsync(q => q.DayNumber == dayNumber);
        if (question == null) return NotFound("No question for today");

        return Ok(question);
    }

    [HttpGet("{id}/answers")]
    public async Task<IActionResult> GetAnswers(int id)
    {
        var answers = await _db.Answers
            .Where(a => a.QuestionId == id)
            .ToListAsync();
        return Ok(answers);
    }

    [HttpPost("{id}/answers")]
    public async Task<IActionResult> SubmitAnswer(int id, [FromBody] string text)
    {
        var userId = User.Identity?.Name ?? "unknown"; // get from auth
        var answer = new Answer
        {
            QuestionId = id,
            UserId = userId,
            Text = text,
            AnsweredAt = DateTime.UtcNow
        };
        _db.Answers.Add(answer);
        await _db.SaveChangesAsync();
        return Ok(answer);
    }
}
