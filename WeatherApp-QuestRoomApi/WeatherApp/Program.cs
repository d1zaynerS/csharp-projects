using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/weather/{city}", async (string city, IHttpClientFactory clientFactory) =>
{
    string apiKey = "5a300bd7bd5dde0069ed2bcca9c13976"; 

    if (true) 
    {
        return Results.Json(new 
        { 
            name = city, 
            main = new { temp = 20.5, humidity = 65 }, 
            weather = new[] { new { description = "ясно (демо-режим)" } } 
        });
    }

    var client = clientFactory.CreateClient();
    string url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&units=metric&appid={apiKey}&lang=ua";
    
    var response = await client.GetAsync(url);
    if (!response.IsSuccessStatusCode)
    {
        return Results.NotFound(new { message = "Місто не знайдено або помилка запиту до погоди." });
    }

    var data = await response.Content.ReadFromJsonAsync<object>();
    return Results.Json(data);
});

app.Run();