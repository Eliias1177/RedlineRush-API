using System.Collections.Generic;

namespace DragRacingAPI.Models
{
    public class Player
    {
        public string Password { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        
        // Moneda del juego para el sistema de economía (MVP)
        public int Cash { get; set; } 
        
        // El garaje del jugador con todos sus autos desbloqueados
        public List<Car> Garage { get; set; } = new List<Car>();
    }
}