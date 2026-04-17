using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragRacingAPI.Models;

namespace DragRacingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarageController : ControllerBase
    {
        private readonly AppDbContext _context; // NUEVO
        public GarageController(AppDbContext context) { _context = context; }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<Player>> GetPlayerGarage(int playerId)
        {
            var player = await _context.Players.Include(p => p.Garage).FirstOrDefaultAsync(p => p.Id == playerId);
            if (player == null) return NotFound(new { message = "Jugador no encontrado." });
            return Ok(player);
        }

        [HttpPost("upgrade/engine")]
        public async Task<ActionResult<Player>> UpgradeEngine([FromBody] int playerId)
        {
            var player = await _context.Players.Include(p => p.Garage).FirstOrDefaultAsync(p => p.Id == playerId);
            if (player == null) return NotFound();
            var car = player.Garage.FirstOrDefault(); 
            if (car == null) return BadRequest("No tienes auto.");
            
            int cost = car.EngineLevel * 2000;
            if (player.Cash < cost) return BadRequest("Cash insuficiente.");

            player.Cash -= cost;
            car.EngineLevel++;
            car.Horsepower += 40; 
            await _context.SaveChangesAsync(); // Guardar cambios asíncronamente
            return Ok(player);
        }
    }
}