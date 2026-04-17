namespace DragRacingAPI.Models
{
    public class LoginRequest { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }
    public class RegisterRequest { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }

    public class RaceResultRequest
    {
        public int PlayerId { get; set; }
        public float TimeSeconds { get; set; }
        // NUEVO: Nivel de dificultad (Novato, Pro, Jefe)
        public string Difficulty { get; set; } = "Novato"; 
    }

    public class BuyCarRequest { public int PlayerId { get; set; } public int CarId { get; set; } }
}