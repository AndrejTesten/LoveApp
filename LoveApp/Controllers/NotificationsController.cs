using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _db;
    public NotificationsController(AppDbContext db)
    {
        _db = db;
    }

    // Get unseen notifications for logged-in user
    [HttpGet("unseen")]
    public async Task<IActionResult> GetUnseenNotifications()
    {
        var userId = User.Identity!.Name!;
        var notifications = await _db.Notifications
            .Where(n => n.UserId == userId && !n.Shown)
            .ToListAsync();

        return Ok(notifications);
    }


    // Mark notification as shown
    [HttpPost("mark-shown/{id}")]
    public async Task<IActionResult> MarkAsShown(int id)
    {
        var notif = await _db.Notifications.FindAsync(id);
        if (notif == null) return NotFound();

        notif.Shown = true;
        await _db.SaveChangesAsync();
        return Ok();
    }
}
