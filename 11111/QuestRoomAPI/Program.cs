using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var cases = new List<DetectiveCase>
{
    new DetectiveCase 
    { 
        Id = 1, 
        Title = "Зникнення професора", 
        Difficulty = "Середня", 
        Status = "розпочато",
        Locations = new List<Location>
        {
            new Location 
            { 
                Id = 101, 
                Name = "Кабінет директора", 
                Clues = new List<Clue>
                {
                    new Clue { Id = 1, Name = "Старий ключик", Description = "Маленький латунний ключ", IsKeyEvidence = true, IsLocked = false },
                    new Clue { Id = 2, Name = "Фальшивий документ", Description = "Побіжний аркуш паперу", IsKeyEvidence = false, IsLocked = false }
                }
            },
            new Location 
            { 
                Id = 102, 
                Name = "Старий гараж", 
                Clues = new List<Clue>
                {
                    new Clue { Id = 3, Name = "Замкнений сейф", Description = "Важкий металевий сейф", IsKeyEvidence = true, IsLocked = true }
                }
            }
        },
        Notebook = new List<string> { "Справа розпочата. Знайдено перші локації." },
        Suspects = new List<string> { "Асистент Джон", "Прибиральниця Марта" }
    }
};

app.MapGet("/api/cases", () => cases.Select(c => new { c.Id, c.Title, c.Difficulty, c.Status }));

app.MapGet("/api/cases/{id}/locations", (int id) =>
{
    var detectiveCase = cases.FirstOrDefault(c => c.Id == id);
    if (detectiveCase == null) return Results.NotFound(new { message = "Справу не знайдено." });
    return Results.Json(detectiveCase.Locations.Select(l => new { l.Id, l.Name }));
});

app.MapGet("/api/locations/{locationId}/clues", (int locationId) =>
{
    var location = cases.SelectMany(c => c.Locations).FirstOrDefault(l => l.Id == locationId);
    if (location == null) return Results.NotFound(new { message = "Локацію не знайдено." });
    return Results.Json(location.Clues);
});

app.MapPost("/api/cases/{id}/inspect", (int id, [FromBody] InspectRequest req) =>
{
    var detectiveCase = cases.FirstOrDefault(c => c.Id == id);
    if (detectiveCase == null) return Results.NotFound(new { message = "Справу не знайдено." });

    var clue = detectiveCase.Locations.SelectMany(l => l.Clues).FirstOrDefault(c => c.Id == req.ClueId);
    if (clue == null) return Results.NotFound(new { message = "Доказ не знайдено." });

    if (clue.IsLocked)
    {
        return Results.BadRequest(new { message = "Цей доказ заблоковано! Потрібно розв'язати загадку." });
    }

    string resultMessage = clue.IsKeyEvidence 
        ? $"Успіх! '{clue.Name}' є ключовою вуликою." 
        : $"Увага: '{clue.Name}' виявився фальшивкою.";

    detectiveCase.Notebook.Add($"Досліджено предмет: {clue.Name}");
    return Results.Json(new { clue.Id, clue.Name, clue.Description, result = resultMessage });
});

app.MapPost("/api/cases/{id}/combine-clues", (int id, [FromBody] CombineRequest req) =>
{
    var detectiveCase = cases.FirstOrDefault(c => c.Id == id);
    if (detectiveCase == null) return Results.NotFound(new { message = "Справу не знайдено." });

    // Проста логіка комбінації (наприклад: Ключ (id=1) + Сейф (id=3))
    if ((req.ClueId1 == 1 && req.ClueId2 == 3) || (req.ClueId1 == 3 && req.ClueId2 == 1))
    {
        // Розблокуємо сейф або додаємо новий факт
        var safe = cases.SelectMany(c => c.Locations).SelectMany(l => l.Clues).FirstOrDefault(c => c.Id == 3);
        if (safe != null) safe.IsLocked = false;

        detectiveCase.Notebook.Add("Успішно об'єднано: Ключ + Старий сейф = Відкритий сейф з новими документами!");
        return Results.Json(new { success = true, message = "Відкрито сейф! Знайдено нові документи." });
    }

    return Results.BadRequest(new { success = false, message = "Ці докази не поєднуються між собою." });
});

app.MapPost("/api/cases/{id}/solve", (int id, [FromBody] SolveRequest req) =>
{
    var detectiveCase = cases.FirstOrDefault(c => c.Id == id);
    if (detectiveCase == null) return Results.NotFound(new { message = "Справу не знайдено." });

    // Перевіряємо правильність (наприклад, злочинець - "Асистент Джон")
    bool isCorrect = req.Suspect.Equals("Асистент Джон", StringComparison.OrdinalIgnoreCase);

    if (isCorrect)
    {
        detectiveCase.Status = "розкрито";
        return Results.Json(new { result = "Перемога!", message = "Ви правильно розкрили справу та назвали злочинця!" });
    }
    else
    {
        return Results.Json(new { result = "Поразка", message = "Неправильний підозрюваний або мотив. Спробуйте ще раз." });
    }
});

app.MapGet("/api/cases/{id}/notebook", (int id) =>
{
    var detectiveCase = cases.FirstOrDefault(c => c.Id == id);
    if (detectiveCase == null) return Results.NotFound(new { message = "Справу не знайдено." });

    return Results.Json(new 
    { 
        detectiveCase.Notebook, 
        detectiveCase.Suspects 
    });
});

app.Run();

public class DetectiveCase
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Difficulty { get; set; } = "";
    public string Status { get; set; } = "";
    public List<Location> Locations { get; set; } = new();
    public List<string> Notebook { get; set; } = new();
    public List<string> Suspects { get; set; } = new();
}

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Clue> Clues { get; set; } = new();
}

public class Clue
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsKeyEvidence { get; set; }
    public bool IsLocked { get; set; }
}

public class InspectRequest
{
    public int ClueId { get; set; }
}

public class CombineRequest
{
    public int ClueId1 { get; set; }
    public int ClueId2 { get; set; }
}

public class SolveRequest
{
    public string Suspect { get; set; } = "";
    public string Motive { get; set; } = "";
}