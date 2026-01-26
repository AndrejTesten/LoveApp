using LoveApp.Data;
using LoveApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- THIS

namespace LoveApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TripsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<List<Trip>> GetTrips()
        {
            return await _db.Trips
                .Include(t => t.Route.OrderBy(p => p.PointOrder))
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip(Trip trip)
        {
            _db.Trips.Add(trip);
            await _db.SaveChangesAsync();
            return Ok(trip);
        }
    }

}
