using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragRacingAPI.Models;

namespace DragRacingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public AuthController(AppDbContext context) 
        { 
            _context = context; 
        }

        [HttpPost("login")]
        public async Task<ActionResult<Player>> Login([FromBody] LoginRequest request)
        {
            var player = await _context.Players
                .Include(p => p.Garage) // ¡CRUCIAL: EF no carga listas automáticamente!
                .FirstOrDefaultAsync(p => p.Username.ToLower() == request.Username.ToLower() && p.Password == request.Password);

            if (player == null) return Unauthorized(new { message = "Datos incorrectos." });
            return Ok(player); 
        }

        [HttpPost("register")]
        public async Task<ActionResult<Player>> Register([FromBody] RegisterRequest request)
        {
            var exists = await _context.Players.AnyAsync(p => p.Username.ToLower() == request.Username.ToLower());
            if (exists) return BadRequest(new { message = "El usuario ya existe." });

            var newPlayer = new Player
            {
                Username = request.Username, 
                Password = request.Password, 
                Cash = 200000, 
                Garage = new System.Collections.Generic.List<Car> {
                    new Car { Name = "VW Golf GTI", MaxRpm = 6500, Horsepower = 220, ShiftTimeMs = 400 }
                }
            };

            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync(); // Guardar en SQLite
            return Ok(newPlayer); 
        }

        // --- NUEVA RUTA PARA PANEL DE ADMINISTRADOR ---
        [HttpGet("all")]
        public async Task<ActionResult> GetAllUsers() 
        { 
            var users = await _context.Players
                .Include(p => p.Garage)
                .ToListAsync();
                
            return Ok(users); 
        }
    }
}