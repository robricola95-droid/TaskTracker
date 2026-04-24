var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/api/tasks", (AppDbContext db) =>
{
    return db.Tasks.ToList();
});

app.MapPost("/api/tasks", (AppDbContext db, TaskItem task) =>
{
    db.Tasks.Add(task);
    db.SaveChanges();
    return task;
});

app.MapDelete("/api/tasks/{id}", (AppDbContext db, int id) =>
{
    var task = db.Tasks.Find(id);
    if (task == null)
        return Results.NotFound();
    
    db.Tasks.Remove(task);
    db.SaveChanges();
    return Results.Ok();
});

app.Run();