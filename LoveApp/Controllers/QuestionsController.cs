using System.Security.Claims;
using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // Get today's active question (random, unanswered by both users)
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentQuestion()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized("User ID missing");

        int userId = int.Parse(userIdClaim.Value);
        int otherUserId = userId == 1 ? 2 : 1;

        // Check for existing active question
        var active = await _db.ActiveQuestions.FirstOrDefaultAsync();

        Question question;

        if (active != null)
        {
            question = await _db.Questions.FindAsync(active.QuestionId);
        }
        else
        {
            // Pick a random question not fully answered by both
            var answeredIds = await _db.Answers
                .GroupBy(a => a.QuestionId)
                .Where(g => g.Select(a => a.UserId).Distinct().Count() == 2)
                .Select(g => g.Key)
                .ToListAsync();

            question = await _db.Questions
                .Where(q => !answeredIds.Contains(q.Id))
                .OrderBy(r => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (question == null)
                return NotFound("All questions answered");

            // Create active question
            _db.ActiveQuestions.Add(new ActiveQuestion { QuestionId = question.Id });
            await _db.SaveChangesAsync();
        }

        // Fetch existing answers for this question
        var answers = await _db.Answers
            .Where(a => a.QuestionId == question.Id)
            .ToListAsync();

        return Ok(new { question, answers });
    }

    public class AnswerDto
    {
        public string Text { get; set; } = string.Empty;
    }


    [HttpPost("{id}/answers")]
    public async Task<IActionResult> SubmitAnswer(int id, [FromBody] AnswerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Text))
            return BadRequest("Answer cannot be empty");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? throw new UnauthorizedAccessException();

        // Save answer
        var answer = new Answer
        {
            QuestionId = id,
            UserId = userId,
            Text = dto.Text,
            AnsweredAt = DateTime.UtcNow
        };
        _db.Answers.Add(answer);
        await _db.SaveChangesAsync();

        // Check if both users answered -> remove active question
        var distinctUsers = await _db.Answers
            .Where(a => a.QuestionId == id)
            .Select(a => a.UserId)
            .Distinct()
            .CountAsync();

        if (distinctUsers >= 2)
        {
            var active = await _db.ActiveQuestions.FirstOrDefaultAsync();
            if (active != null && active.QuestionId == id)
            {
                _db.ActiveQuestions.Remove(active);
                await _db.SaveChangesAsync();
            }
        }

        return Ok(answer);
    }




    // Get all answered questions with both answers
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAnswered()
    {
        var questions = await _db.Questions
            .Where(q => _db.Answers
                .Where(a => a.QuestionId == q.Id)
                .Select(a => a.UserId)
                .Distinct()
                .Count() == 2)
            .ToListAsync();

        var result = new List<object>();
        foreach (var q in questions)
        {
            var answers = await _db.Answers
                .Where(a => a.QuestionId == q.Id)
                .Select(a => new { a.UserId, a.Text })
                .ToListAsync();

            result.Add(new { question = q, answers });
        }

        return Ok(result);
    }
}
