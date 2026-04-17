using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragRacingAPI.Models;

namespace DragRacingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class RaceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RaceController(AppDbContext context) { _context = context; }

        [HttpPost("finish")]
        public async Task<ActionResult<Player>> FinishRace([FromBody] RaceResultRequest request)
        {
            var player = await _context.Players.Include(p => p.Garage).FirstOrDefaultAsync(p => p.Id == request.PlayerId);
            if (player == null) return NotFound(new { message = "Jugador no encontrado." });

            int baseReward = request.Difficulty switch { "Profesional" => 600, "Jefe de Calle" => 2000, _ => 200 };
            if (request.TimeSeconds < 12f) baseReward += 500; else if (request.TimeSeconds < 15f) baseReward += 200;

            player.Cash += baseReward;
            await _context.SaveChangesAsync();
            return Ok(player);
        }
    }
}