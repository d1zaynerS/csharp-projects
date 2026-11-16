using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", (IUserRepository repo) =>
{
    var users = repo.GetAll();
    var html = "<h1>Список користувачів</h1><a href='/add'>Додати користувача</a><ul>";
    foreach (var u in users)
    {
        html += $"<li>{u.Name} (ID: {u.Id}) - <a href='/edit?id={u.Id}'>Редагувати</a> | <a href='/delete?id={u.Id}'>Видалити</a></li>";
    }
    html += "</ul>";
    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/add", () =>
{
    var html = "<h1>Додати</h1><form method='POST' action='/add'>Ім'я: <input name='name'/><button type='submit'>Зберегти</button></form>";
    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapPost("/add", async (HttpContext context, IUserRepository repo) =>
{
    var form = await context.Request.ReadFormAsync();
    var name = form["name"].ToString();
    repo.Add(new User { Name = name });
    context.Response.Redirect("/");
});

app.MapGet("/edit", (string id, IUserRepository repo) =>
{
    var user = repo.GetById(id);
    if (user == null) return Results.Content("Користувача з таким ID не існує або ID невірний.", "text/html; charset=utf-8");

    var html = $"<h1>Редагувати</h1><form method='POST' action='/edit'><input type='hidden' name='id' value='{user.Id}'/>Ім'я: <input name='name' value='{user.Name}'/><button type='submit'>Оновити</button></form>";
    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapPost("/edit", async (HttpContext context, IUserRepository repo) =>
{
    var form = await context.Request.ReadFormAsync();
    repo.Update(form["id"].ToString(), form["name"].ToString());
    context.Response.Redirect("/");
});

app.MapGet("/delete", (string id, IUserRepository repo, HttpContext context) =>
{
    repo.Delete(id);
    context.Response.Redirect("/");
});

var products = new List<Product>();
int nextProductId = 1;

bool IsAuthorized(HttpRequest request)
{
    var token = request.Headers["Authorization"].ToString();
    return token == "MySecretToken123";
}

app.MapGet("/api/products", (HttpRequest request) =>
{
    if (!IsAuthorized(request)) return Results.Unauthorized();
    return Results.Json(products);
});

app.MapGet("/api/products/{id:int}", (int id, HttpRequest request) =>
{
    if (!IsAuthorized(request)) return Results.Unauthorized();
    var product = products.FirstOrDefault(p => p.Id == id);
    return product != null ? Results.Json(product) : Results.NotFound();
});

app.MapPost("/api/products/add", async (HttpRequest request) =>
{
    if (!IsAuthorized(request)) return Results.Unauthorized();
    
    var form = await request.ReadFormAsync();
    var product = new Product { Id = nextProductId++, Name = form["name"].ToString() };
    products.Add(product);
    return Results.Json(product);
});

app.MapPost("/api/products/delete", async (HttpRequest request) =>
{
    if (!IsAuthorized(request)) return Results.Unauthorized();
    
    var form = await request.ReadFormAsync();
    if (int.TryParse(form["id"], out int id))
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            products.Remove(product);
            return Results.Json(new { Message = "Продукт видалено" });
        }
    }
    return Results.BadRequest(new { Message = "Помилка видалення" });
});

app.MapPost("/upload", async (HttpRequest request, IWebHostEnvironment env) =>
{
    var file = request.Form.Files.FirstOrDefault();
    if (file == null || file.Length == 0) return Results.BadRequest("Файл не вибрано.");

    var uploadsFolder = Path.Combine(env.WebRootPath, "uploads");
    Directory.CreateDirectory(uploadsFolder); 
    
    var filePath = Path.Combine(uploadsFolder, file.FileName);

    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    var fileBytes = ms.ToArray();
    File.WriteAllBytes(filePath, fileBytes);

    var fileUrl = $"/uploads/{file.FileName}";
    return Results.Json(new { Url = fileUrl, Message = "Файл успішно збережено!" });
});

app.Run();

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}

public interface IUserRepository
{
    void Add(User user);
    void Delete(string id);
    User? GetById(string id);
    void Update(string id, string newName);
    List<User> GetAll();
}

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public void Add(User user) => _users.Add(user);
    public void Delete(string id) => _users.RemoveAll(u => u.Id == id);
    public User? GetById(string id) => _users.FirstOrDefault(u => u.Id == id);
    public void Update(string id, string newName)
    {
        var user = GetById(id);
        if (user != null) user.Name = newName;
    }
    public List<User> GetAll() => _users;
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
