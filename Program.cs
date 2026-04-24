var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

var tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Title = "Learn C#", Completed = false },
    new TaskItem { Id = 2, Title = "Build API", Completed = false }
};

app.MapGet("/api/tasks", () =>
{
    return tasks;
});

app.MapPost("/api/tasks", (TaskItem task) =>
{
    tasks.Add(task);
    return task;
});

app.Run();