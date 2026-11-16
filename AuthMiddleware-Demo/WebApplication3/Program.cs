var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var books = new List<Book>
{
    new Book("Гаррі Поттер", "Дж. Роулінг", "fantasy"),
    new Book("Історія музики", "Й. Бах", "music"),
    new Book("Теорія гармонії", "М. Скорик", "music"),
    new Book("Дюна", "Ф. Герберт", "sci-fi")
};

app.UseMiddleware<AuthMiddleware>();


app.Map("/allbooks", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync($"<h2>Усі книги:</h2>{BuildHtmlTable(books)}");
    });
});

app.Map("/getbooks", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        string category = context.Request.Query["category"].ToString().ToLower();

        var filteredBooks = books.Where(b => b.Category.ToLower() == category);

        await context.Response.WriteAsync($"<h2>Книги категорії '{category}':</h2>{BuildHtmlTable(filteredBooks)}");
    });
});


app.Run(async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync("<h3>Перейдіть за адресою /allbooks або /getbooks?token=token12345&category=music</h3>");
});

app.Run();


string BuildHtmlTable(IEnumerable<Book> bookList)
{
    string html = "<table border='1' cellpadding='8' style='border-collapse: collapse;'>" +
                  "<tr><th>Назва</th><th>Автор</th><th>Категорія</th></tr>";

    foreach (var b in bookList)
    {
        html += $"<tr><td>{b.Title}</td><td>{b.Author}</td><td>{b.Category}</td></tr>";
    }

    html += "</table>";
    return html;
}

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/getbooks"))
        {
            var token = context.Request.Query["token"];

            if (token != "token12345")
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("<h3>Доступ заборонено: Невірний або відсутній токен!</h3>");
                return;
            }
        }

        await _next(context);
    }
}

public record Book(string Title, string Author, string Category);