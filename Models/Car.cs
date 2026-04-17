using System.ComponentModel.DataAnnotations;

namespace DragRacingAPI.Models
{
    public class Car
    {
        [Key] // ¡NUEVO!
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxRpm { get; set; }
        public int Horsepower { get; set; }
        public int ShiftTimeMs { get; set; }
        public int EngineLevel { get; set; } = 1;
        
        // EF Core necesita esto para la relación
        public int? PlayerId { get; set; } 
    }
}