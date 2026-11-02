using wt_lab6_6minimalapi_rabtsevich.Data;
using wt_lab6_6minimalapi_rabtsevich.Models;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────
//  Swagger / OpenAPI
// ─────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "WT Lab 6 — Flights API",
        Version = "v1",
        Description = "REST API для управления рейсами (ASP.NET Core Minimal API, ЛР-6)"
    });
});

var app = builder.Build();

// Swagger UI доступен всегда (и локально, и на Render)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Flights API v1");
    options.RoutePrefix = "swagger";
});

// ─────────────────────────────────────────
//  БЛОК 1: диагностические эндпоинты
// ─────────────────────────────────────────
app.MapGet("/", () => "Minimal API работает!");

app.MapGet("/api/info", () => new
{
    Application = "Web Technologies",
    Technology  = "ASP.NET Core Minimal API",
    Version     = "1.0",
    Domain      = "Авиаперевозки"
});

// ─────────────────────────────────────────
//  БЛОК 3: GET endpoints для рейсов
// ─────────────────────────────────────────

// GET /api/flights?search=...
app.MapGet("/api/flights", (string? search) =>
{
    var q = FlightStore.Flights.AsEnumerable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        q = q.Where(f =>
            f.FlightNumber.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            f.Departure.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            f.Destination.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    return Results.Ok(q.ToList());
});

// GET /api/flights/{id}
app.MapGet("/api/flights/{id:int}", (int id) =>
{
    var flight = FlightStore.GetFlight(id);
    return flight is null ? Results.NotFound() : Results.Ok(flight);
});

// ─────────────────────────────────────────
//  БЛОК 4: POST
// ─────────────────────────────────────────
app.MapPost("/api/flights", (Flight flight) =>
{
    if (string.IsNullOrWhiteSpace(flight.FlightNumber) ||
        string.IsNullOrWhiteSpace(flight.Departure) ||
        string.IsNullOrWhiteSpace(flight.Destination))
    {
        return Results.BadRequest(new { error = "Поля FlightNumber, Departure, Destination обязательны." });
    }

    if (FlightStore.FlightNumberExists(flight.FlightNumber))
    {
        return Results.BadRequest(new { error = "Рейс с таким номером уже существует." });
    }

    flight.Id = FlightStore.NextFlightId();
    flight.FlightNumber = flight.FlightNumber.Trim().ToUpperInvariant();

    FlightStore.Flights.Add(flight);

    return Results.Created($"/api/flights/{flight.Id}", flight);
});

// ─────────────────────────────────────────
//  БЛОК 5: PUT и DELETE
// ─────────────────────────────────────────
app.MapPut("/api/flights/{id:int}", (int id, Flight updated) =>
{
    var flight = FlightStore.GetFlight(id);
    if (flight is null) return Results.NotFound();

    if (!string.IsNullOrWhiteSpace(updated.FlightNumber) &&
        FlightStore.FlightNumberExists(updated.FlightNumber, excludeId: id))
    {
        return Results.BadRequest(new { error = "Другой рейс уже использует такой номер." });
    }

    flight.FlightNumber   = updated.FlightNumber?.Trim().ToUpperInvariant() ?? flight.FlightNumber;
    flight.Departure      = updated.Departure;
    flight.Destination    = updated.Destination;
    flight.DepartureTime  = updated.DepartureTime;
    flight.TicketPrice    = updated.TicketPrice;
    flight.SeatsAvailable = updated.SeatsAvailable;
    flight.IsActive       = updated.IsActive;
    flight.AirlineId      = updated.AirlineId;
    flight.AircraftId     = updated.AircraftId;
    flight.FlightType     = updated.FlightType;

    return Results.Ok(flight);
});

app.MapDelete("/api/flights/{id:int}", (int id) =>
{
    var flight = FlightStore.GetFlight(id);
    if (flight is null) return Results.NotFound();

    FlightStore.Flights.Remove(flight);
    return Results.NoContent();
});

// ─────────────────────────────────────────
//  Справочники
// ─────────────────────────────────────────
app.MapGet("/api/airlines", () => Results.Ok(FlightStore.Airlines));
app.MapGet("/api/aircrafts", () => Results.Ok(FlightStore.Aircrafts));

// ─────────────────────────────────────────
//  Запуск: порт из переменной окружения PORT
// ─────────────────────────────────────────
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");