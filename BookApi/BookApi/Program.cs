using BookApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
