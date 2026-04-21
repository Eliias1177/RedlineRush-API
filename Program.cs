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
    // Aquí puedes agregar el código de 'Seeding' de la Fase 14 para los autos iniciales
}

app.UseSwagger(); app.UseSwaggerUI();
app.MapControllers();
app.MapHub<RaceHub>("/racehub");
app.Run();