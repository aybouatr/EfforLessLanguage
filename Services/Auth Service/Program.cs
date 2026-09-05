var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/users", () =>
{
    return new[]
    {
        new { Id = 1, Name = "Ayoub" },
        new { Id = 2, Name = "John" }
    };
});

app.Run();