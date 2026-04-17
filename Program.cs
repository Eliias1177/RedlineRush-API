using Microsoft.EntityFrameworkCore;
using DragRacingAPI;
using DragRacingAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAR EF CORE CON SQLITE (NUEVO)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=racing.db";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. CREAR Y SEEDAR LA BASE DE DATOS AUTOMÁTICAMENTE (NUEVO)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Crea la DB si no existe (racing.db)
    context.Database.EnsureCreated();

    // Seed: Catálogo de la tienda
    if (!context.CatalogCars.Any())
    {
        context.CatalogCars.AddRange(new List<Car>
        {
            new Car { Name = "VW Golf GTI (MOCK)", MaxRpm = 6500, Horsepower = 220, ShiftTimeMs = 400, EngineLevel = 1 },
            new Car { Name = "Ford Mustang GT", MaxRpm = 7000, Horsepower = 460, ShiftTimeMs = 250, EngineLevel = 1 },
            new Car { Name = "Nissan GT-R Nismo", MaxRpm = 7100, Horsepower = 600, ShiftTimeMs = 200, EngineLevel = 1 },
            new Car { Name = "McLaren P1", MaxRpm = 8000, Horsepower = 903, ShiftTimeMs = 150, EngineLevel = 1 }
        });
        context.SaveChanges();
    }

    // Seed: Jugador inicial (RacerX)
    if (!context.Players.Any())
    {
        context.Players.Add(new Player
        {
            Username = "RacerX", Password = "123", Cash = 5000,
            Garage = new List<Car> { 
                // El Golf GTI base del catálogo
                new Car { Name = "VW Golf GTI", MaxRpm = 6500, Horsepower = 220, ShiftTimeMs = 400, EngineLevel = 1 }
            }
        });
        context.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();