using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddControllers();

var connectionString = Environment.GetEnvironmentVariable("ConnectionString") 
    ?? "Host=localhost;Username=postgres;Password=password;Database=tasktracker;SslMode=Require";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("Database initialized successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DB init warning (non-critical): {ex.Message}");
        // Continue anyway - table might already exist
    }
}

app.MapGet("/api/tasks", (AppDbContext db) => db.Tasks.ToList());
app.MapPost("/api/tasks", (AppDbContext db, TaskItem task) =>
{
    db.Tasks.Add(task);
    db.SaveChanges();
    return task;
});
app.MapDelete("/api/tasks/{id}", (AppDbContext db, int id) =>
{
    var task = db.Tasks.Find(id);
    if (task == null) return Results.NotFound();
    db.Tasks.Remove(task);
    db.SaveChanges();
    return Results.Ok();
});

app.Urls.Add("http://0.0.0.0:5116");
app.Run();
