using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_rabtsevich.Data;
using wt_lab7_7postgres_efcore_rabtsevich.Models;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────
//  Swagger / OpenAPI
// ─────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "WT Lab 7 — Flights API (PostgreSQL + EF Core)",
        Version = "v1",
        Description = "REST API для управления рейсами (EF Core, Code First, миграции)"
    });
});

// ─────────────────────────────────────────
//  Строка подключения: DATABASE_URL (Render) или ConnectionStrings:DefaultConnection
// ─────────────────────────────────────────
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
                  ?? builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    // Render передаёт DATABASE_URL вида: postgres://user:pass@host:port/db
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    var builderCs = new Npgsql.NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.IsDefaultPort ? 5432 : uri.Port,
        Database = uri.AbsolutePath.TrimStart('/'),
        Username = Uri.UnescapeDataString(userInfo[0]),
        Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "",
        // Внутренняя сеть Render — TLS не требуется.
        // Для внешнего подключения заменить на SslMode.Require
        SslMode = Npgsql.SslMode.Disable
    };
    connectionString = builderCs.ConnectionString;
}

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException(
        "Не задана строка подключения: отсутствует ConnectionStrings:DefaultConnection и DATABASE_URL.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// ─────────────────────────────────────────
//  Автоматическое применение миграций при старте
//  ⚠ НЕ использовать вместе с EnsureCreated()
// ─────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    app.Logger.LogInformation("PostgreSQL: миграции применены.");
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "Flights API v1");
    o.RoutePrefix = "swagger";
});

// ─────────────────────────────────────────
//  Диагностические эндпоинты
// ─────────────────────────────────────────
app.MapGet("/", () => "Minimal API (PostgreSQL + EF Core) работает!");

app.MapGet("/api/info", () => new
{
    Application = "Web Technologies",
    Technology  = "ASP.NET Core Minimal API + EF Core + PostgreSQL",
    Version     = "1.0",
    Domain      = "Авиаперевозки"
});

// ═════════════════════════════════════════
//  FLIGHTS — CRUD через AppDbContext
// ═════════════════════════════════════════

// GET /api/flights?search=...
app.MapGet("/api/flights", async (string? search, AppDbContext db) =>
{
    var query = db.Flights.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        var s = search.ToLower();
        query = query.Where(f =>
            f.FlightNumber.ToLower().Contains(s) ||
            f.Departure.ToLower().Contains(s) ||
            f.Destination.ToLower().Contains(s));
    }

    var result = await query.OrderBy(f => f.Id).ToListAsync();
    return Results.Ok(result);
});

// GET /api/flights/{id}
app.MapGet("/api/flights/{id:int}", async (int id, AppDbContext db) =>
{
    var flight = await db.Flights.FirstOrDefaultAsync(f => f.Id == id);
    return flight is null ? Results.NotFound() : Results.Ok(flight);
});

// POST /api/flights
app.MapPost("/api/flights", async (Flight flight, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(flight.FlightNumber) ||
        string.IsNullOrWhiteSpace(flight.Departure) ||
        string.IsNullOrWhiteSpace(flight.Destination))
    {
        return Results.BadRequest(new { error = "Поля FlightNumber, Departure, Destination обязательны." });
    }

    var normalized = flight.FlightNumber.Trim().ToUpperInvariant();
    var exists = await db.Flights.AnyAsync(f => f.FlightNumber == normalized);
    if (exists)
        return Results.BadRequest(new { error = "Рейс с таким номером уже существует." });

    flight.Id = 0; // назначается БД
    flight.FlightNumber = normalized;
    flight.CreatedAt = DateTime.UtcNow;

    db.Flights.Add(flight);
    await db.SaveChangesAsync();

    return Results.Created($"/api/flights/{flight.Id}", flight);
});

// PUT /api/flights/{id}
app.MapPut("/api/flights/{id:int}", async (int id, Flight updated, AppDbContext db) =>
{
    var flight = await db.Flights.FirstOrDefaultAsync(f => f.Id == id);
    if (flight is null) return Results.NotFound();

    if (!string.IsNullOrWhiteSpace(updated.FlightNumber))
    {
        var normalized = updated.FlightNumber.Trim().ToUpperInvariant();
        var conflict = await db.Flights.AnyAsync(f => f.Id != id && f.FlightNumber == normalized);
        if (conflict)
            return Results.BadRequest(new { error = "Другой рейс уже использует такой номер." });

        flight.FlightNumber = normalized;
    }

    flight.Departure      = updated.Departure;
    flight.Destination    = updated.Destination;
    flight.DepartureTime  = updated.DepartureTime;
    flight.TicketPrice    = updated.TicketPrice;
    flight.SeatsAvailable = updated.SeatsAvailable;
    flight.IsActive       = updated.IsActive;
    flight.AirlineId      = updated.AirlineId;
    flight.AircraftId     = updated.AircraftId;
    flight.FlightType     = updated.FlightType;

    await db.SaveChangesAsync();
    return Results.Ok(flight);
});

// DELETE /api/flights/{id}
app.MapDelete("/api/flights/{id:int}", async (int id, AppDbContext db) =>
{
    var flight = await db.Flights.FirstOrDefaultAsync(f => f.Id == id);
    if (flight is null) return Results.NotFound();

    db.Flights.Remove(flight);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ═════════════════════════════════════════
//  LINQ-запросы (п.19 методички)
// ═════════════════════════════════════════

// 1) Фильтрация: рейсы дороже 10000
app.MapGet("/api/flights/expensive", async (AppDbContext db) =>
    Results.Ok(await db.Flights
        .Where(f => f.TicketPrice > 10000)
        .OrderByDescending(f => f.TicketPrice)
        .ToListAsync()));

// 2) Сортировка: по цене возрастанию
app.MapGet("/api/flights/sorted-by-price", async (AppDbContext db) =>
    Results.Ok(await db.Flights
        .OrderBy(f => f.TicketPrice)
        .ToListAsync()));

// 3) Выборка с условием: активные, с местами, отсортированы по названию
app.MapGet("/api/flights/available", async (AppDbContext db) =>
    Results.Ok(await db.Flights
        .Where(f => f.IsActive && f.SeatsAvailable > 0)
        .OrderBy(f => f.FlightNumber)
        .ToListAsync()));

// 4) Собственный: группировка по авиакомпании + подсчёт рейсов
app.MapGet("/api/flights/stats-by-airline", async (AppDbContext db) =>
{
    var stats = await db.Flights
        .GroupBy(f => f.AirlineId)
        .Select(g => new
        {
            AirlineId = g.Key,
            FlightCount = g.Count(),
            MinPrice = g.Min(f => f.TicketPrice),
            MaxPrice = g.Max(f => f.TicketPrice),
            AvgSeats = g.Average(f => f.SeatsAvailable)
        })
        .ToListAsync();
    return Results.Ok(stats);
});

// ═════════════════════════════════════════
//  Справочники (только чтение; в БД через seed)
// ═════════════════════════════════════════
app.MapGet("/api/airlines", async (AppDbContext db) =>
    Results.Ok(await db.Airlines.ToListAsync()));

app.MapGet("/api/aircrafts", async (AppDbContext db) =>
    Results.Ok(await db.Aircrafts.ToListAsync()));

// ─────────────────────────────────────────
//  Запуск: порт из переменной окружения PORT
// ─────────────────────────────────────────
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");