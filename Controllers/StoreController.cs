using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragRacingAPI.Models;

namespace DragRacingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StoreController(AppDbContext context) { _context = context; }

        [HttpGet("catalog")]
        public async Task<ActionResult<List<Car>>> GetCatalog() { return Ok(await _context.CatalogCars.ToListAsync()); }

        [HttpPost("buy")]
        public async Task<ActionResult<Player>> BuyCar([FromBody] BuyCarRequest request)
        {
            var player = await _context.Players.Include(p => p.Garage).FirstOrDefaultAsync(p => p.Id == request.PlayerId);
            if (player == null) return NotFound("Jugador no encontrado.");
            var carToBuy = await _context.CatalogCars.FirstOrDefaultAsync(c => c.Id == request.CarId);
            if (carToBuy == null) return NotFound("Auto no encontrado.");
            
            int price = carToBuy.Horsepower * 25;
            if (player.Cash < price) return BadRequest("Cash insuficiente.");

            player.Cash -= price;
            player.Garage.Insert(0, new Car { // Clonar auto del catálogo a SQLite
                Name = carToBuy.Name, MaxRpm = carToBuy.MaxRpm, Horsepower = carToBuy.Horsepower, ShiftTimeMs = carToBuy.ShiftTimeMs, EngineLevel = 1
            });
            await _context.SaveChangesAsync();
            return Ok(player);
        }
    }
}