using Microsoft.EntityFrameworkCore;
using DragRacingAPI;
using DragRacingAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=racing.db"));
builder.Services.AddControllers();
builder.Services.AddSingleton<DragRacingAPI.Hubs.MatchmakingService>();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!context.CatalogCars.Any())
    {
        context.CatalogCars.AddRange(
            // ¡OJO! Si tu clase se llama 'Car' en vez de 'CatalogCar', cámbialo aquí abajo
            new CatalogCar { Name = "Nissan 350z (Z33)", Horsepower = 306, MaxRpm = 7500, ShiftTimeMs = 350 },
            new CatalogCar { Name = "Toyota Supra MK4", Horsepower = 320, MaxRpm = 6800, ShiftTimeMs = 400 },
            new CatalogCar { Name = "Porsche 911 GT3 RS", Horsepower = 518, MaxRpm = 9000, ShiftTimeMs = 150 },
            new CatalogCar { Name = "Honda Civic Type R", Horsepower = 306, MaxRpm = 7000, ShiftTimeMs = 250 },
            new CatalogCar { Name = "Mazda RX-7 (FD)", Horsepower = 255, MaxRpm = 8000, ShiftTimeMs = 300 }
        );
        context.SaveChanges();
    }
    // Aquí puedes agregar el código de 'Seeding' de la Fase 14 para los autos iniciales
}

app.UseSwagger(); app.UseSwaggerUI();
app.MapControllers();
app.MapHub<RaceHub>("/racehub");
app.Run();