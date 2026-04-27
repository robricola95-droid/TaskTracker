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
        Console.WriteLine($"DB init warning: {ex.Message}");
    }
}

app.MapGet("/api/tasks", (AppDbContext db) =>
{
    try
    {
        return Results.Ok(db.Tasks.ToList());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"GET error: {ex.Message}");
        return Results.StatusCode(500);
    }
});

app.MapPost("/api/tasks", (AppDbContext db, TaskItem task) =>
{
    try
    {
        task.Id = 0;
        db.Tasks.Add(task);
        db.SaveChanges();
        return Results.Created($"/api/tasks/{task.Id}", task);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"POST error: {ex.Message}");
        return Results.StatusCode(500);
    }
});

app.MapPut("/api/tasks/{id}", (AppDbContext db, int id, TaskItem updateTask) =>
{
    try
    {
        var task = db.Tasks.Find(id);
        if (task == null) return Results.NotFound();
        task.Completed = updateTask.Completed;
        task.Title = updateTask.Title;
        db.SaveChanges();
        return Results.Ok(task);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"PUT error: {ex.Message}");
        return Results.StatusCode(500);
    }
});

app.MapDelete("/api/tasks/{id}", (AppDbContext db, int id) =>
{
    try
    {
        var task = db.Tasks.Find(id);
        if (task == null) return Results.NotFound();
        db.Tasks.Remove(task);
        db.SaveChanges();
        return Results.Ok();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DELETE error: {ex.Message}");
        return Results.StatusCode(500);
    }
});

app.Urls.Add("http://0.0.0.0:5116");
app.Run();