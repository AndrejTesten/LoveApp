using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- THIS

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OurMapController : ControllerBase
{
    private readonly AppDbContext _db;

    public OurMapController(AppDbContext db)
    {
        _db = db;
    }

    // Get all pins with images
    [HttpGet]
    public async Task<IActionResult> GetPins()
    {
        var pins = await _db.MemoryPins
            .Include(p => p.Images)
            .ToListAsync();

        // Convert image bytes to base64 strings for Angular
        var result = pins.Select(p => new
        {
            p.Id,
            p.Title,
            p.Text,
            p.Lat,
            p.Lng,
            Images = p.Images.Select(i => $"data:image/jpeg;base64,{Convert.ToBase64String(i.ImageData)}").ToList()
        });

        return Ok(result);
    }

    // Add new pin with images
    [HttpPost]
    public async Task<IActionResult> AddPin([FromBody] NewPinDto dto)
    {
        var pin = new MemoryPin
        {
            Title = dto.Title,
            Text = dto.Text,
            Lat = dto.Lat,
            Lng = dto.Lng,
            Images = dto.Images.Select(img => new MemoryImage
            {
                FileName = img.FileName,
                ImageData = Convert.FromBase64String(img.Base64Data)
            }).ToList()
        };

        _db.MemoryPins.Add(pin);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePin(int id)
    {
        // Find the pin including images
        var pin = await _db.MemoryPins
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pin == null)
            return NotFound("Pin not found");

        // Remove images first (EF Core cascade might handle this, but safer)
        _db.MemoryImages.RemoveRange(pin.Images);

        // Remove the pin
        _db.MemoryPins.Remove(pin);

        await _db.SaveChangesAsync();

        return Ok(new { message = "Pin deleted successfully" });
    }
}

// DTO for receiving images from Angular
public class NewPinDto
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public List<ImageDto> Images { get; set; } = new();
}

public class ImageDto
{
    public string FileName { get; set; } = string.Empty;
    public string Base64Data { get; set; } = string.Empty;
}
