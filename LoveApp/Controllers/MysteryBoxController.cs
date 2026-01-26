//using Microsoft.AspNetCore.Mvc;
//using YourApp.Models; // MysteryBoxSentence
//using Microsoft.EntityFrameworkCore;
//using LoveApp.Data;

//[Route("api/[controller]")]
//[ApiController]
//public class MysteryBoxController : ControllerBase
//{
//    private readonly AppDbContext _context;

//    public MysteryBoxController(AppDbContext context)
//    {
//        _context = context;
//    }

//    // GET: api/mysterybox/random
//    [HttpGet("random")]
//    public async Task<IActionResult> GetRandomSentence()
//    {
//        var count = await _context.MysteryBoxSentences.CountAsync();
//        if (count == 0) return NotFound("No sentences in DB");

//        var rand = new Random();
//        int skip = rand.Next(count);
//        var sentence = await _context.MysteryBoxSentences.Skip(skip).FirstOrDefaultAsync();

//        return Ok(sentence);
//    }
//}
