//using Microsoft.AspNetCore.Mvc;
//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;
//using YourApp.Models; // MemoryPin, MemoryImage
//using Microsoft.EntityFrameworkCore;
//using LoveApp.Data;

//[Route("api/[controller]")]
//[ApiController]
//public class MemoryController : ControllerBase
//{
//    private readonly AppDbContext _context;
//    private readonly Cloudinary _cloudinary;

//    public MemoryController(AppDbContext context, Cloudinary cloudinary)
//    {
//        _context = context;
//        _cloudinary = cloudinary;
//    }

//    // GET: api/memory
//    [HttpGet]
//    public async Task<IActionResult> GetAllPins()
//    {
//        var pins = await _context.MemoryPins
//            .Include(p => p.Images)
//            .ToListAsync();
//        return Ok(pins);
//    }

//    // GET: api/memory/{id}
//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetPin(int id)
//    {
//        var pin = await _context.MemoryPins
//            .Include(p => p.Images)
//            .FirstOrDefaultAsync(p => p.Id == id);

//        if (pin == null) return NotFound();
//        return Ok(pin);
//    }

//    // POST: api/memory/upload-image
//    [HttpPost("upload-image")]
//    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
//    {
//        if (file == null || file.Length == 0)
//            return BadRequest("No file uploaded");

//        using var stream = file.OpenReadStream();
//        var uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
//        {
//            File = new FileDescription(file.FileName, stream),
//            Folder = "memories"
//        });

//        return Ok(new { url = uploadResult.SecureUrl.ToString() });
//    }

//    // POST: api/memory
//    [HttpPost]
//    public async Task<IActionResult> AddPin([FromBody] MemoryPin pin)
//    {
//        _context.MemoryPins.Add(pin);
//        await _context.SaveChangesAsync();
//        return Ok(pin);
//    }

//    // PUT: api/memory/{id}
//    [HttpPut("{id}")]
//    public async Task<IActionResult> EditPin(int id, [FromBody] MemoryPin updatedPin)
//    {
//        var pin = await _context.MemoryPins
//            .Include(p => p.Images)
//            .FirstOrDefaultAsync(p => p.Id == id);

//        if (pin == null) return NotFound();

//        pin.Title = updatedPin.Title;
//        pin.Text = updatedPin.Text;
//        // optionally update images
//        await _context.SaveChangesAsync();
//        return Ok(pin);
//    }

//    // DELETE: api/memory/{id}
//    [HttpDelete("{id}")]
//    public async Task<IActionResult> DeletePin(int id)
//    {
//        var pin = await _context.MemoryPins.FindAsync(id);
//        if (pin == null) return NotFound();

//        _context.MemoryPins.Remove(pin);
//        await _context.SaveChangesAsync();
//        return Ok();
//    }
//}
